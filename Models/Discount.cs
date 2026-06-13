namespace APBD_PROJEKT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Discount
{
    [Key]
    public int DiscountId { get; set; }
    
    
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string OfferType { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal Value { get; set; }
    
    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DateFrom { get; set; }
    
    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime DateTo { get; set; }
    
    public ICollection<SoftwareDiscount> SoftwareDiscounts { get; set; } = new List<SoftwareDiscount>();
}