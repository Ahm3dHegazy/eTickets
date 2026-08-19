using System.Security.Claims;
using System.Text.Json;
using eTickets.Business.Interfaces;
using eTickets.Data;
using eTickets.Data.Extensions;
using eTickets.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Business.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private const string LegacySessionKey = "ShoppingCart";
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDbContext context;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public IReadOnlyList<ShoppingCartItem> GetItems()
        {
            var cart = GetCart(createIfMissing: false);
            return cart == null ? Array.Empty<ShoppingCartItem>() : cart.Items.AsReadOnly();
        }

        public void AddToCart(int movieId, string movieName, string movieImageUrl, decimal price, int quantity = 1)
        {
            if (quantity <= 0) return;

            var cart = GetCart(createIfMissing: true)!;
            var existing = cart.Items.FirstOrDefault(item => item.MovieId == movieId);
            if (existing != null)
                existing.Quantity += quantity;
            else
            {
                cart.Items.Add(new ShoppingCartItem
                {
                    MovieId = movieId,
                    MovieName = movieName,
                    MovieImageURL = movieImageUrl,
                    Price = price,
                    Quantity = quantity
                });
            }

            cart.UpdatedAt = DateTime.UtcNow;
            context.SaveChanges();
        }

        public void RemoveFromCart(int movieId, int quantity = 1)
        {
            var cart = GetCart(createIfMissing: false);
            var existing = cart?.Items.FirstOrDefault(item => item.MovieId == movieId);
            if (existing == null || cart == null) return;

            existing.Quantity -= quantity;
            if (existing.Quantity <= 0)
                context.ShoppingCartItems.Remove(existing);

            cart.UpdatedAt = DateTime.UtcNow;
            context.SaveChanges();
        }

        public decimal GetTotal() => GetItems().Sum(item => item.Price * item.Quantity);

        public int GetCount() => GetItems().Sum(item => item.Quantity);

        public void ClearCart()
        {
            var cart = GetCart(createIfMissing: false);
            if (cart == null || cart.Items.Count == 0) return;

            context.ShoppingCartItems.RemoveRange(cart.Items);
            cart.UpdatedAt = DateTime.UtcNow;
            context.SaveChanges();
        }

        private Cart? GetCart(bool createIfMissing)
        {
            var session = httpContextAccessor.HttpContext?.Session
                ?? throw new InvalidOperationException("Session is not available.");
            var key = GetCartKey(session);
            var cart = context.Carts.Include(existing => existing.Items).SingleOrDefault(existing => existing.CartKey == key);

            if (cart == null && createIfMissing)
            {
                cart = new Cart { CartKey = key };
                context.Carts.Add(cart);
            }

            var legacyItems = ReadLegacyItems(session);
            if (legacyItems.Count > 0)
            {
                cart ??= new Cart { CartKey = key };
                if (context.Entry(cart).State == EntityState.Detached)
                    context.Carts.Add(cart);

                foreach (var legacyItem in legacyItems)
                {
                    var existing = cart.Items.FirstOrDefault(item => item.MovieId == legacyItem.MovieId);
                    if (existing != null)
                        existing.Quantity += legacyItem.Quantity;
                    else
                        cart.Items.Add(legacyItem);
                }

                cart.UpdatedAt = DateTime.UtcNow;
                context.SaveChanges();
                session.Remove(LegacySessionKey);
            }

            return cart;
        }

        private string GetCartKey(ISession session)
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrWhiteSpace(userId) ? $"session:{session.Id}" : $"user:{userId}";
        }

        private static List<ShoppingCartItem> ReadLegacyItems(ISession session)
        {
            var data = session.GetString(LegacySessionKey);
            return string.IsNullOrWhiteSpace(data)
                ? new List<ShoppingCartItem>()
                : JsonSerializer.Deserialize<List<ShoppingCartItem>>(data) ?? new List<ShoppingCartItem>();
        }
    }
}
