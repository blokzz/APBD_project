namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("ContractPayments")]
public class ContractPayments
{
    [Key]
    public int PaymentId { get; set; }
    
    public int ContractId { get; set; }
    
    public Contract Contract { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime PaidAt { get; set; }
}