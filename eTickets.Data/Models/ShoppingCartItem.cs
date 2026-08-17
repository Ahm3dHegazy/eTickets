namespace eTickets.Data.Models
{
    public class ShoppingCartItem
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieImageURL { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => Price * Quantity;
    }
}