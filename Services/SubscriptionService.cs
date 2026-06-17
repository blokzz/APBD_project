using APBD_PROJEKT.Dtos;
using APBD_PROJEKT.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace APBD_PROJEKT.Services;
using Data;
using Models;
using External;
public class SubscriptionService : ISubscriptionService
{
    private readonly AppDbContext _context;

    public SubscriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateSubscription(CreateSubscriptionDto dto)
    {
        if (dto.RenewalPeriodMonths < 1 || dto.RenewalPeriodMonths > 24)
            throw new ArgumentException("Okres odnowienia musi wynosić od 1 do 24 miesięcy");
        
        var client = await _context.Clients.FindAsync(dto.ClientId);
        if (client == null)
            throw new NotFoundException("Klient nie istnieje");
        
        var software = await _context.Softwares.FindAsync(dto.SoftwareId);
        if (software == null)
            throw new NotFoundException("Oprogramowanie nie istnieje");
        
        var existingSub = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.ClientId == dto.ClientId
                && s.SoftwareId == dto.SoftwareId
                && s.IsActive);
        if (existingSub != null)
            throw new InvalidOperationException("Klient ma już aktywną subskrypcję na ten produkt");
        
        var basePrice = software.YearlyLicensePrice;
        
        var bestDiscount = await _context.Discounts
            .Where(d => d.OfferType == "subscription"
                && d.DateFrom <= DateTime.Now
                && d.DateTo >= DateTime.Now
                && d.SoftwareDiscounts.Any(sd => sd.SoftwareId == dto.SoftwareId))
            .OrderByDescending(d => d.Value)
            .FirstOrDefaultAsync();

        var firstPaymentPrice = basePrice;
        if (bestDiscount != null)
            firstPaymentPrice *= (1 - bestDiscount.Value / 100m);
        
        var isLoyal = await IsReturningClient(dto.ClientId);
        if (isLoyal)
            firstPaymentPrice *= 0.95m;

        var subscription = new Subscription
        {
            ClientId = dto.ClientId,
            SoftwareId = dto.SoftwareId,
            Name = dto.Name,
            RenewalPeriodMonths = dto.RenewalPeriodMonths,
            Price = basePrice,
            StartDate = DateTime.Now,
            IsActive = true
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        
        _context.SubscriptionPayments.Add(new SubscriptionPayment
        {
            SubscriptionId = subscription.SubscriptionId,
            Amount = firstPaymentPrice,
            PaidAt = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    public async Task PayRenewal(int subscriptionId, decimal amount)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.SubscriptionPayments)
            .FirstOrDefaultAsync(s => s.SubscriptionId == subscriptionId);

        if (subscription == null)
            throw new NotFoundException("Subskrypcja nie istnieje");

        if (!subscription.IsActive)
            throw new InvalidOperationException("Subskrypcja jest anulowana");
        
        var lastPayment = subscription.SubscriptionPayments
            .OrderByDescending(p => p.PaidAt)
            .FirstOrDefault();

        if (lastPayment == null)
            throw new InvalidOperationException("Brak historii płatności");

        var lastPeriodStart = lastPayment.PaidAt;
        var nextPeriodStart = lastPeriodStart.AddMonths(subscription.RenewalPeriodMonths);
        var nextPeriodEnd = nextPeriodStart.AddMonths(subscription.RenewalPeriodMonths);
        
        if (DateTime.Now > nextPeriodEnd)
        {
            subscription.IsActive = false;
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Subskrypcja anulowana — brak płatności za poprzedni okres");
        }
        
        if (DateTime.Now < nextPeriodStart)
            throw new InvalidOperationException("Bieżący okres jest już opłacony");
        
        var renewalPrice = subscription.Price;
        var isLoyal = await IsReturningClient(subscription.ClientId);
        if (isLoyal)
            renewalPrice *= 0.95m;
        
        if (amount != renewalPrice)
            throw new ArgumentException($"Nieprawidłowa kwota. Wymagana: {renewalPrice}");

        _context.SubscriptionPayments.Add(new SubscriptionPayment
        {
            SubscriptionId = subscriptionId,
            Amount = amount,
            PaidAt = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    private async Task<bool> IsReturningClient(int clientId)
    {
        var hasContract = await _context.Contracts
            .AnyAsync(c => c.ClientId == clientId && c.IsSigned);

        var hasSubscription = await _context.Subscriptions
            .AnyAsync(s => s.ClientId == clientId && s.IsActive);

        return hasContract || hasSubscription;
    }
}