using APBD_PROJEKT.Dtos.Clients;

namespace apbd.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using APBD_PROJEKT.Models;
using APBD_PROJEKT.Services;
using APBD_PROJEKT.Dtos;
using APBD_PROJEKT.Data;
using APBD_PROJEKT.Exceptions;

public class ClientServiceTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateIndividualClient_ValidData_CreatesClient()
    {
        var context = GetInMemoryContext();
        var service = new ClientService(context);
        var dto = new CreateIndividualClientDto
        {
            FirstName = "Jan", LastName = "Kowalski",
            Address = "ul. Test", Email = "jan@test.pl",
            Phone = "123456789", PESEL = "12345678901"
        };

        await service.CreateIndividualClient(dto);

        var client = await context.IndividualClients.FirstOrDefaultAsync();
        Assert.NotNull(client);
        Assert.Equal("Jan", client.FirstName);
    }

    [Fact]
    public async Task CreateIndividualClient_InvalidPESEL_ThrowsException()
    {
        var context = GetInMemoryContext();
        var service = new ClientService(context);
        var dto = new CreateIndividualClientDto
        {
            FirstName = "Jan", LastName = "Kowalski",
            Address = "ul. Test", Email = "jan@test.pl",
            Phone = "123456789", PESEL = "123"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateIndividualClient(dto));
    }
}