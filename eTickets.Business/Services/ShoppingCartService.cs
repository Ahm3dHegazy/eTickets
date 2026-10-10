using System.Security.Claims;
using System.Text;
using System.Text.Json;
using eTickets.Business.Interfaces;
using eTickets.Data;
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

            if (existing != null)  // If the item already exists in the cart, update its quantity
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

        public async Task MergeCartAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return;

            var session = httpContextAccessor.HttpContext?.Session
                ?? throw new InvalidOperationException("Session is not available.");

            // 1) Merge any DB-backed session cart (session:{session.Id}) into the user's cart
            var sessionCartKey = $"session:{session.Id}";
            var userCartKey = $"user:{userId}";

            var sessionCart = await context.Carts
                .Include(c => c.Items)
                .SingleOrDefaultAsync(c => c.CartKey == sessionCartKey);

            var userCart = await context.Carts
                .Include(c => c.Items)
                .SingleOrDefaultAsync(c => c.CartKey == userCartKey);

            if (sessionCart != null && sessionCart.Items.Count > 0)
            {
                if (userCart == null)
                {
                    userCart = new Cart { CartKey = userCartKey, CreatedAt = DateTime.UtcNow };
                    context.Carts.Add(userCart);
                }

                foreach (var item in sessionCart.Items.ToList())
                {
                    var existing = userCart.Items.FirstOrDefault(i => i.MovieId == item.MovieId);
                    if (existing != null)
                    {
                        existing.Quantity += item.Quantity;
                        // keep existing.Price or choose to overwrite: existing.Price = item.Price;
                    }
                    else
                    {
                        userCart.Items.Add(new ShoppingCartItem
                        {
                            MovieId = item.MovieId,
                            MovieName = item.MovieName,
                            MovieImageURL = item.MovieImageURL,
                            Price = item.Price,
                            Quantity = item.Quantity
                        });
                    }
                }

                // remove the session cart and its items
                context.ShoppingCartItems.RemoveRange(sessionCart.Items);
                context.Carts.Remove(sessionCart);

                userCart.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }

            // 2) Also handle older JSON-based legacy session storage (if present)
            var legacyItems = ReadLegacyItems(session);
            if (legacyItems != null && legacyItems.Count > 0)
            {
                userCart ??= new Cart { CartKey = userCartKey, CreatedAt = DateTime.UtcNow };
                if (context.Entry(userCart).State == EntityState.Detached)
                    context.Carts.Add(userCart);

                foreach (var legacyItem in legacyItems)
                {
                    var existing = userCart.Items.FirstOrDefault(i => i.MovieId == legacyItem.MovieId);
                    if (existing != null)
                        existing.Quantity += legacyItem.Quantity;
                    else
                        userCart.Items.Add(new ShoppingCartItem
                        {
                            MovieId = legacyItem.MovieId,
                            MovieName = legacyItem.MovieName,
                            MovieImageURL = legacyItem.MovieImageURL,
                            Price = legacyItem.Price,
                            Quantity = legacyItem.Quantity
                        });
                }

                userCart.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                session.Remove(LegacySessionKey);
            }
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
            if (!string.IsNullOrWhiteSpace(userId))
                return $"user:{userId}";

            if (!session.TryGetValue("CartSessionStarted", out _))
            {
                session.Set("CartSessionStarted", Encoding.UTF8.GetBytes("1"));
            }

            return $"session:{session.Id}";
        }

        private static List<ShoppingCartItem> ReadLegacyItems(ISession session)
        {
            if (session.TryGetValue(LegacySessionKey, out var bytes))
            {
                var data = Encoding.UTF8.GetString(bytes);
                return string.IsNullOrWhiteSpace(data)
                    ? new List<ShoppingCartItem>()
                    : JsonSerializer.Deserialize<List<ShoppingCartItem>>(data) ?? new List<ShoppingCartItem>();
            }

            return new List<ShoppingCartItem>();
        }
    }
}
