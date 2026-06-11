using APBD_PROJEKT.Dtos.Clients;
using  APBD_PROJEKT.Models;
using Microsoft.EntityFrameworkCore;
using APBD_PROJEKT.Data;
namespace APBD_PROJEKT.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context) => _context = context;
    public async Task<IEnumerable<ClientDto>> GetClients()
    {
        var companyClients = await _context.CompanyClients.Select(client => new CompanyClientDto
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            Phone = client.Phone,
            CompanyName = client.CompanyName,
            KRS = client.KRS
        }).ToListAsync();
        
        var individualClients = await _context.IndividualClients.Select(client => new IndividualClientDto()
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            Phone = client.Phone,
            FirstName =  client.FirstName,
            LastName =  client.LastName,
            PESEL =  client.PESEL
        }).ToListAsync();

        List<ClientDto> clientList= new List<ClientDto>();
        clientList.AddRange(companyClients);
        clientList.AddRange(individualClients);
        
        return clientList;
    }
}