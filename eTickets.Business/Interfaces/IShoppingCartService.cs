using eTickets.Data.Models;

namespace eTickets.Business.Interfaces
{
    public interface IShoppingCartService
    {
        IReadOnlyList<ShoppingCartItem> GetItems();
        void AddToCart(int movieId, string movieName, string movieImageUrl, decimal price, int quantity = 1);
        void RemoveFromCart(int movieId, int quantity = 1);
        decimal GetTotal();
        int GetCount();
        void ClearCart();
    }
}