namespace APBD_PROJEKT.Models;

public abstract class Client
{
    
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; }= null!;
    public string Phone { get; set; }= null!;
    
    public bool IsDeleted { get; set; } = false;
    
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}