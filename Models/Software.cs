namespace APBD_PROJEKT.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("Software")]
public class Software
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string CurrentVersion { get; set; } = null!;
    public string Category { get; set; } = null!;
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public ICollection<SoftwareDiscount> SoftwareDiscounts { get; set; } = new List<SoftwareDiscount>();
} 