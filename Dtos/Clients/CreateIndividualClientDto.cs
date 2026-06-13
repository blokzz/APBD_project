namespace APBD_PROJEKT.Dtos.Clients;

public class CreateIndividualClientDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string PESEL { get; set; } = null!;
}