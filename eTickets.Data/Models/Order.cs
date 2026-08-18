using eTickets.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.Models
{
    public class Order : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; } 
    }
}