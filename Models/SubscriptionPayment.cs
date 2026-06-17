namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class SubscriptionPayment
{
    [Key]
    public int PaymentId { get; set; }
    
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }
    
    [Column(TypeName = "datetime2")]
    public DateTime PaidAt { get; set; }
}