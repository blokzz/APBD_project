namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("IndividualClients")]
public class IndividualClient : Client
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PESEL { get; set; } = null!;
    public bool IsDeleted { get; set; }
}