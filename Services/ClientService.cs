using System.Text.RegularExpressions;
using APBD_PROJEKT.Dtos.Clients;
using  APBD_PROJEKT.Models;
using Microsoft.EntityFrameworkCore;
using APBD_PROJEKT.Data;
namespace APBD_PROJEKT.Services;
using  System.Text.RegularExpressions;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context) => _context = context;
    public async Task<IEnumerable<ClientDto>> GetClients()
    {
        var companyClients = await _context.CompanyClients.Where(c => !c.IsDeleted).Select(client => new CompanyClientDto
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            Phone = client.Phone,
            CompanyName = client.CompanyName,
            KRS = client.KRS
        }).ToListAsync();
        
        var individualClients = await _context.IndividualClients.Where(c => !c.IsDeleted).Select(client => new IndividualClientDto()
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

    public async Task CreateIndividualClient(CreateIndividualClientDto client)
    {
        if (client.Phone.Length != 9)
        {
            throw new ArgumentException("Phone length must be 9 characters long");
        }
        if (!Regex.IsMatch(client.PESEL, @"^\d{11}$"))
            throw new ArgumentException("Nieprawidłowy PESEL");
        
        var newClient = new IndividualClient
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            PESEL = client.PESEL,
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address
        };
        
        await _context.IndividualClients.AddAsync(newClient);
        await _context.SaveChangesAsync();
    }
    public async Task CreateCompanyClient(CreateCompanyClientDto client)
    {
        
        if (client.Phone.Length != 9)
        {
            throw new ArgumentException("Phone length must be 9 characters long");
        }
        if (!Regex.IsMatch(client.KRS, @"^\d{10}$"))
            throw new ArgumentException("Nieprawidłowy krs");
        var newClient = new CompanyClient
        {
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address,
            CompanyName = client.CompanyName,
            KRS = client.KRS
        };
        
        await _context.CompanyClients.AddAsync(newClient);
        await _context.SaveChangesAsync();

    }
    public async Task DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
    
        if (client == null)
            throw new KeyNotFoundException("Klient nie istnieje");

        if (client is IndividualClient individual)
        {
            individual.FirstName = "deleted";
            individual.LastName = "deleted";
            individual.Email = "deleted";
            individual.Phone = "deleted";
            individual.Address = "deleted";
            individual.IsDeleted = true;
        }
        else if (client is CompanyClient)
        {
            throw new InvalidOperationException("Danych firm nie można usuwać");
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateClient(int id, UpdateClientDto dto)
    {
        var client = await _context.Clients.FindAsync(id);
    
        if (client == null)
            throw new KeyNotFoundException("Klient nie istnieje");
        
        if (dto.Address != null) client.Address = dto.Address;
        if (dto.Email != null) client.Email = dto.Email;
        if (dto.Phone != null) client.Phone = dto.Phone;

        if (client is IndividualClient individual)
        {
            if (individual.IsDeleted)
                throw new InvalidOperationException("Nie można edytować usuniętego klienta");
        
            if (dto.FirstName != null) individual.FirstName = dto.FirstName;
            if (dto.LastName != null) individual.LastName = dto.LastName;
        }
        else if (client is CompanyClient company)
        {
            if (dto.CompanyName != null) company.CompanyName = dto.CompanyName;
        }

        await _context.SaveChangesAsync();
    }
}