using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        // a GUID/session key used to associate the cart with the client (no auth in project)
        [Required]
        public string CartKey { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public List<ShoppingCartItem> Items { get; set; } = new();
    }
}