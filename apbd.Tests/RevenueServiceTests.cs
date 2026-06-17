namespace APBD_PROJEKT.apbd.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using APBD_PROJEKT.Models;
using APBD_PROJEKT.Services;
using APBD_PROJEKT.Dtos;
using APBD_PROJEKT.Data;
using APBD_PROJEKT.Exceptions;

public class RevenueServiceTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetCurrentRevenue_OnlySignedContracts()
    {
        var context = GetInMemoryContext();
        context.Contracts.Add(new Contract
        {
            ContractId = 1, ClientId = 1, SoftwareId = 1,
            SoftwareVersion = "1.0", Price = 1000,
            DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(10),
            IsSigned = true, IsActive = true, UpdatedSupportYears = 1
        });
        context.Contracts.Add(new Contract
        {
            ContractId = 2, ClientId = 1, SoftwareId = 2,
            SoftwareVersion = "1.0", Price = 2000,
            DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(10),
            IsSigned = false, IsActive = true, UpdatedSupportYears = 1
        });
        await context.SaveChangesAsync();

        var service = new RevenueService(context);
        var revenue = await service.GetCurrentRevenue(null, "PLN");

        Assert.Equal(1000, revenue);
    }
}