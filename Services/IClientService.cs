using APBD_PROJEKT.Dtos.Clients;

namespace APBD_PROJEKT.Services;

public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetClients();
}