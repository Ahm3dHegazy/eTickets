using System.ComponentModel.DataAnnotations;

namespace eTickets.web.ViewModels;

public class CapturePayPalOrderRequest : PayPalCheckoutRequest
{
    [Required] public string PayPalOrderId { get; set; } = string.Empty;
}
