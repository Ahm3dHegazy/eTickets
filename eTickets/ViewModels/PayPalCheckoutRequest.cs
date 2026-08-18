using System.ComponentModel.DataAnnotations;

namespace eTickets.web.ViewModels;

public class PayPalCheckoutRequest
{
    [Required] public string CustomerName { get; set; } = string.Empty;
    [Required, EmailAddress] public string CustomerEmail { get; set; } = string.Empty;
}
