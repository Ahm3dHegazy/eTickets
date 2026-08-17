using eTickets.Data.Models;

namespace eTickets.Business.Interfaces
{
    public interface IShoppingCartService
    {
        IReadOnlyList<ShoppingCartItem> GetItems();
        void AddToCart(int movieId, string movieName, string movieImageUrl, double price, int quantity = 1);
        void RemoveFromCart(int movieId, int quantity = 1);
        double GetTotal();
        int GetCount();
        void ClearCart();
    }
}