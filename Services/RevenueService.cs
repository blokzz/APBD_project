using APBD_PROJEKT.Dtos;
using APBD_PROJEKT.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace APBD_PROJEKT.Services;
using Data;
using Models;
using External;
public class RevenueService : IRevenueService
{
    private readonly AppDbContext _context;

    public RevenueService(AppDbContext context) => _context = context;
    
public async Task<decimal> GetCurrentRevenue(int? softwareId, string currency)
{
    var contractQuery = _context.Contracts.Where(c => c.IsSigned);
    if (softwareId.HasValue)
        contractQuery = contractQuery.Where(c => c.SoftwareId == softwareId.Value);
    var contractRevenue = await contractQuery.SumAsync(c => c.Price);
    
    var subPaymentQuery = _context.SubscriptionPayments
        .Where(sp => sp.Subscription.IsActive);
    if (softwareId.HasValue)
        subPaymentQuery = subPaymentQuery.Where(sp => sp.Subscription.SoftwareId == softwareId.Value);
    var subscriptionRevenue = await subPaymentQuery.SumAsync(sp => sp.Amount);

    return await ConvertCurrency(contractRevenue + subscriptionRevenue, currency);
}

public async Task<decimal> GetForecastRevenue(int? softwareId, string currency)
{
    var contractQuery = _context.Contracts.AsQueryable();
    if (softwareId.HasValue)
        contractQuery = contractQuery.Where(c => c.SoftwareId == softwareId.Value);
    var contractRevenue = await contractQuery.SumAsync(c => c.Price);
    
    var subQuery = _context.Subscriptions.Where(s => s.IsActive);
    if (softwareId.HasValue)
        subQuery = subQuery.Where(s => s.SoftwareId == softwareId.Value);
    
    var subPaymentQuery = _context.SubscriptionPayments
        .Where(sp => sp.Subscription.IsActive);
    if (softwareId.HasValue)
        subPaymentQuery = subPaymentQuery.Where(sp => sp.Subscription.SoftwareId == softwareId.Value);
    var currentSubRevenue = await subPaymentQuery.SumAsync(sp => sp.Amount);
    
    var subscriptions = await subQuery.ToListAsync();
    var projectedRevenue = 0m;
    foreach (var sub in subscriptions)
    {
        var renewalsPerYear = 12 / sub.RenewalPeriodMonths;
        projectedRevenue += sub.Price * renewalsPerYear;
    }

    return await ConvertCurrency(contractRevenue + currentSubRevenue + projectedRevenue, currency);
}
    
    private async Task<decimal> ConvertCurrency(decimal amountPln, string currency)
    {
        if (currency == "PLN") return amountPln;
    
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(
            $"https://api.nbp.pl/api/exchangerates/rates/A/{currency}/?format=json");
    
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<NbpResponse>(response, options);
        var rate = data.Rates[0].Mid;
    
        return amountPln / rate;
    }
}