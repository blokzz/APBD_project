namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("Subscription")]
public class Subscription
{
    [Key]
    public int SubscriptionId { get; set; }
    
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    
    public int SoftwareId { get; set; }
    public Software Software { get; set; } = null!;
    
    [Required]
    public string Name { get; set; } = null!;
    
    public int RenewalPeriodMonths { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    [Column(TypeName = "datetime2")]
    public DateTime StartDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    public ICollection<SubscriptionPayment> SubscriptionPayments { get; set; } = new List<SubscriptionPayment>();
}