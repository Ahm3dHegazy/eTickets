using System.ComponentModel.DataAnnotations;

namespace eTickets.web.ViewModels
{
    public class ContactViewModel
    {
        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(120)]
        public string Subject { get; set; } = string.Empty;

        [Required, StringLength(2000, MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = string.Empty;
    }
}
