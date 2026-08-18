using Microsoft.AspNetCore.Identity;

namespace eTickets.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
