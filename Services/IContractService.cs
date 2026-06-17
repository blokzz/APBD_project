namespace APBD_PROJEKT.Services;
using Dtos;
public interface IContractService
{
    Task CreateContract(CreateContractDto contract);
    
    Task AddPayment(int contractId , decimal amount);
}