using APBD_PROJEKT.Dtos;
using APBD_PROJEKT.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.Services;
using Data;
using Models;
public class ContractService : IContractService
{
    private readonly AppDbContext _context;

    public ContractService(AppDbContext context) => _context = context;
    public async Task CreateContract(CreateContractDto contract)
    {
        var softwareExists = await _context.Softwares.Where(s => s.Id == contract.SoftwareId).FirstOrDefaultAsync();
        var clientExists = await _context.Clients.Where(c => c.Id == contract.ClientId
                                                             && (c is IndividualClient &&
                                                                 !(c as IndividualClient)!.IsDeleted))
            .FirstOrDefaultAsync();

        if (softwareExists==null || clientExists==null)
        {
            throw new NotFoundException("client or software not found");
        }
        
        var contractAlreadyExists = await _context.Contracts.Where(c=>c.ClientId==contract.ClientId && c.SoftwareId ==contract.SoftwareId  && c.IsActive).FirstOrDefaultAsync();
        if (contractAlreadyExists!=null)
        {
            throw new ConflictException("contract already exists");
        }
        
        var validSupportedYears = contract.UpdateSupportYears >=1 && contract.UpdateSupportYears<=4;
        if (!validSupportedYears)
        {
            throw new ConflictException("support years must be between 1 and 4");
        }
        
        var isValidDate = (contract.DateTo - contract.DateFrom) <= TimeSpan.FromDays((double)30) && (contract.DateTo - contract.DateFrom) >= TimeSpan.FromDays((double)3);
        if (!isValidDate)
        {
            throw new ConflictException($"Invalid contract date: {contract.DateFrom}-{contract.DateTo}");
        }
        
        var specialClient = await _context.Contracts.AnyAsync(c => c.ClientId == contract.ClientId);
        
        var discount = await _context.Discounts
            .Where(d => d.SoftwareDiscounts.Any(sd => sd.SoftwareId == contract.SoftwareId)
                        && d.OfferType == "contract"
                        && d.DateFrom <= DateTime.Now
                        && d.DateTo >= DateTime.Now)
            .OrderByDescending(d => d.Value)
            .FirstOrDefaultAsync();

        var discountValue = discount?.Value ?? 0;

        decimal priceTotal = 0;
        
        if (discountValue > 0)
        {
            priceTotal = softwareExists.YearlyLicensePrice 
                         * (1 - discountValue / 100m) 
                         * (specialClient ? 0.95m : 1m) 
                         + (contract.UpdateSupportYears - 1) * 1000m;
        }

        var finalContract = new Contract
        {
            ClientId = contract.ClientId,
            SoftwareId = contract.SoftwareId,
            SoftwareVersion = contract.SoftwareVersion,
            DateFrom = contract.DateFrom,
            DateTo = contract.DateTo,
            Price = priceTotal,
            UpdatedSupportYears = contract.UpdateSupportYears,
            IsSigned = false,
            IsActive = true

        };
        _context.Contracts.Add(finalContract);
        await _context.SaveChangesAsync();
    }
    
    
    public async Task AddPayment(int contractId, decimal amount)
    {
        var contract = await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.ContractId == contractId);

        if (contract == null)
            throw new NotFoundException("Kontrakt nie istnieje");

        if (DateTime.Now > contract.DateTo)
            throw new InvalidOperationException("Termin płatności minął");

        if (!contract.IsActive)
            throw new InvalidOperationException("Kontrakt nie jest aktywny");

        var paidSoFar = contract.Payments.Sum(p => p.Amount);
    
        if (paidSoFar + amount > contract.Price)
            throw new InvalidOperationException("Płatność przekracza wartość kontraktu");

        _context.ContractPayments.Add(new ContractPayments
        {
            ContractId = contractId,
            Amount = amount,
            PaidAt = DateTime.Now
        });

        if (paidSoFar + amount == contract.Price)
            contract.IsSigned = true;

        await _context.SaveChangesAsync();
    }
}