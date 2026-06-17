namespace APBD_PROJEKT.External;

public class NbpResponse
{
    public string Table { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public string Code { get; set; } = null!;
    public List<NbpRate> Rates { get; set; } = new();
}
