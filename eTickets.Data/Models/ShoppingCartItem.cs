using eTickets.Models;
using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        // FK to Cart
        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        // FK to Movie (existing model)
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public string MovieName { get; set; }
        public string MovieImageURL { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}