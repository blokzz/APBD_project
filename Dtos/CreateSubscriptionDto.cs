namespace APBD_PROJEKT.Dtos;

public class CreateSubscriptionDto
{
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public string Name { get; set; } = null!;
    public int RenewalPeriodMonths { get; set; }
}