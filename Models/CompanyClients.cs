namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations.Schema;
[Table("CompanyClients")]
public class CompanyClient : Client
{
    public string CompanyName { get; set; }= null!;
    public string KRS { get; set; }= null!;
}