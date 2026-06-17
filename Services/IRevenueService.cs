namespace APBD_PROJEKT.Services;

public interface IRevenueService
{
    Task<decimal> GetCurrentRevenue(int? softwareId, string currency);
    Task<decimal> GetForecastRevenue(int? softwareId, string currency);
}