namespace APBD_PROJEKT.Dtos.Clients;

public abstract class ClientDto
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
