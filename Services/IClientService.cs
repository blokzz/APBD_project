using APBD_PROJEKT.Dtos.Clients;

namespace APBD_PROJEKT.Services;

public interface IClientService
{
    Task<IEnumerable<object>> GetClients();
    
    Task CreateIndividualClient(CreateIndividualClientDto client);
    Task CreateCompanyClient(CreateCompanyClientDto client);
    Task DeleteClient(int id);
    Task UpdateClient(int id, UpdateClientDto dto);
}