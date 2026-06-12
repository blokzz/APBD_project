namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("Contract")]
public class Contract
{
    [Key]
    public int ContractId { get; set; }
    
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    
    public int SoftwareId { get; set; }
    public Software Software { get; set; } = null!;
    
    public string SoftwareVersion { get; set; } = null!;
    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DateFrom { get; set; }
    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DateTo { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public int UpdatedSupportYears { get; set; }
    
    public bool IsSigned { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public ICollection<ContractPayments> Payments { get; set; } = new List<ContractPayments>();
}