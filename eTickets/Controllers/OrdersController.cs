using Microsoft.AspNetCore.Mvc;
using eTickets.Business.Interfaces;
using eTickets.Data.Models;

namespace eTickets.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;
        private readonly IShoppingCartService cartService;

        public OrdersController(IOrdersService ordersService, IShoppingCartService cartService)
        {
            this.ordersService = ordersService;
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await ordersService.GetAllAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await ordersService.GetByIdAsync(id);
            if (order == null)
                return View("NotFound");

            return View(order);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var items = cartService.GetItems();
            if (!items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromCart(string customerName, string customerEmail)
        {
            var cartItems = cartService.GetItems();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Movies");

            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(customerEmail))
            {
                ModelState.AddModelError("", "Please provide your name and email to complete the order.");
                return View("Checkout");
            }

            var order = new Order
            {
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                OrderDate = DateTime.UtcNow,
                TotalPrice = cartItems.Sum(i => i.Price * i.Quantity),
                OrderItems = cartItems.Select(i => new OrderItem
                {
                    MovieId = i.MovieId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Price
                }).ToList()
            };

            await ordersService.AddAsync(order);
            await ordersService.SaveAsync();

            cartService.ClearCart();

            TempData["StatusMessage"] = "Order placed successfully.";
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await ordersService.DeleteAsync(id);
            await ordersService.SaveAsync();

            TempData["StatusMessage"] = "Order deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}