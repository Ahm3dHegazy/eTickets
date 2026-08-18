using System.Text.Json;
using eTickets.Business.Interfaces;
using eTickets.Data.Extensions;
using eTickets.Data.Models;
using Microsoft.AspNetCore.Http;

namespace eTickets.Business.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private const string SessionKey = "ShoppingCart";
        private readonly ISession _session;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext?.Session
                ?? throw new InvalidOperationException("Session is not available.");
        }

        private List<ShoppingCartItem> Load()
        {
            var data = _session.GetString(SessionKey);
            return string.IsNullOrEmpty(data)
                ? new List<ShoppingCartItem>()
                : JsonSerializer.Deserialize<List<ShoppingCartItem>>(data) ?? new List<ShoppingCartItem>();
        }

        private void Save(List<ShoppingCartItem> items)
        {
            _session.SetString(SessionKey, JsonSerializer.Serialize(items));
        }

        public IReadOnlyList<ShoppingCartItem> GetItems()
        {
            return Load().AsReadOnly();
        }

        public void AddToCart(int movieId, string movieName, string movieImageUrl, decimal price, int quantity = 1)
        {
            if (quantity <= 0) return;

            var items = Load();
            var existing = items.FirstOrDefault(i => i.MovieId == movieId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                items.Add(new ShoppingCartItem
                {
                    MovieId = movieId,
                    MovieName = movieName,
                    MovieImageURL = movieImageUrl,
                    Price = price,
                    Quantity = quantity
                });
            }

            Save(items);
        }

        public void RemoveFromCart(int movieId, int quantity = 1)
        {
            var items = Load();
            var existing = items.FirstOrDefault(i => i.MovieId == movieId);
            if (existing == null) return;

            existing.Quantity -= quantity;
            if (existing.Quantity <= 0)
                items.Remove(existing);

            Save(items);
        }

        public decimal GetTotal()
        {
            return Load().Sum(i => i.Price * i.Quantity);
        }

        public int GetCount()
        {
            return Load().Sum(i => i.Quantity);
        }

        public void ClearCart()
        {
            _session.Remove(SessionKey);
        }
    }
}