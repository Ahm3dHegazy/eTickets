using eTickets.Data.Base;
using eTickets.Models;

namespace eTickets.Data.Models
{
    public class OrderItem : IEntityBase
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}