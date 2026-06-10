namespace APBD_PROJEKT.Models;

public abstract class Client
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; }= null!;
    public string Phone { get; set; }= null!;
}