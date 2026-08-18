namespace eTickets.Configuration;

public class PayPalOptions
{
    public const string SectionName = "PayPal";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public bool UseSandbox { get; set; } = true;
    public string Currency { get; set; } = "USD";
}
