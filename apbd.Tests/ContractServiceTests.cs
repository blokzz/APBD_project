namespace apbd.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using APBD_PROJEKT.Models;
using APBD_PROJEKT.Services;
using APBD_PROJEKT.Dtos;
using APBD_PROJEKT.Data;
using APBD_PROJEKT.Exceptions;

public class ContractServiceTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
    
    [Fact]
    public async Task CreateContract_ClientNotFound_ThrowsNotFoundException()
    {
        var context = GetInMemoryContext();
        context.Softwares.Add(new Software { Id = 1, Name = "Test", YearlyLicensePrice = 1000 , Category = "cat1" , CurrentVersion = "2.1" , Description = "desc1"});
        await context.SaveChangesAsync();

        var service = new ContractService(context);
        var dto = new CreateContractDto { ClientId = 999, SoftwareId = 1, DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(5) };

        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateContract(dto));
    }
    
    
        [Theory]
        [InlineData(1000, 1, 0, false, 1000)]      // bazowa cena, bez zniżek
        [InlineData(1000, 2, 0, false, 2000)]      // +1 rok wsparcia
        [InlineData(1000, 3, 0, false, 3000)]      // +2 lata wsparcia
        [InlineData(1000, 4, 0, false, 4000)]      // +3 lata wsparcia
        [InlineData(1000, 1, 10, false, 900)]      // 10% zniżki
        [InlineData(1000, 1, 25, false, 750)]      // 25% zniżki
        [InlineData(1000, 1, 0, true, 950)]        // powracający klient 5%
        [InlineData(1000, 1, 10, true, 855)]       // 10% + 5% powracający
        [InlineData(2000, 2, 20, true, 2280)]      // 2000+1000=3000, *0.8=2400, *0.95=2280
        public void CalculatePrice_VariousInputs_ReturnsCorrectValue(
            decimal basePrice, int supportYears, decimal discount, bool returning, decimal expected)
        {
            var price = basePrice + (supportYears - 1) * 1000m;
            price *= (1 - discount / 100m);
            if (returning) price *= 0.95m;

            Assert.Equal(expected, price);
        }
    
}
