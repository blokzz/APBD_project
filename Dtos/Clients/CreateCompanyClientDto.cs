namespace APBD_PROJEKT.Dtos.Clients;

public class CreateCompanyClientDto
{
    public string CompanyName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string KRS { get; set; } = null!;
}