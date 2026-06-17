namespace APBD_PROJEKT.Services;
using Dtos;
public interface ISubscriptionService
{
    Task CreateSubscription(CreateSubscriptionDto dto);
    Task PayRenewal(int subscriptionId, decimal amount);
}