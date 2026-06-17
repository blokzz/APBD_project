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
        var query = _context.Contracts.Where(c => c.IsSigned);
    
        if (softwareId.HasValue)
            query = query.Where(c => c.SoftwareId == softwareId.Value);
    
        var contractRevenue = await query.SumAsync(c => c.Price);
    
        // subsrykbcje jeszcze
    
        return await ConvertCurrency(contractRevenue, currency);
    }
    
    public async Task<decimal> GetForecastRevenue(int? softwareId, string currency)
    {
        var query = _context.Contracts.AsQueryable();
    
        if (softwareId.HasValue)
            query = query.Where(c => c.SoftwareId == softwareId.Value);
    
        var contractRevenue = await query.SumAsync(c => c.Price);
    
        return await ConvertCurrency(contractRevenue, currency);
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