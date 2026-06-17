namespace APBD_PROJEKT.Dtos;

public class CreateContractDto
{
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public string SoftwareVersion { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int UpdateSupportYears { get; set; }
}