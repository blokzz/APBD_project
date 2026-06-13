namespace APBD_PROJEKT.Models;

public class SoftwareDiscount
{
    public int SoftwareId { get; set; }
    public Software Software { get; set; } = null!;
    
    public int DiscountId { get; set; }
    public Discount Discount { get; set; } = null!;
}