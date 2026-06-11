namespace APBD_PROJEKT.Dtos.Clients;

public class IndividualClientDto : ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PESEL { get; set; }
}