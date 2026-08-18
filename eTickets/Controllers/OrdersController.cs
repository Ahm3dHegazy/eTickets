using Microsoft.AspNetCore.Mvc;
using eTickets.Business.Interfaces;
using eTickets.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using eTickets.Services;
using eTickets.web.ViewModels;

namespace eTickets.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;
        private readonly IShoppingCartService cartService;
        private readonly PayPalService payPalService;

        public OrdersController(IOrdersService ordersService, IShoppingCartService cartService, PayPalService payPalService)
        {
            this.ordersService = ordersService;
            this.cartService = cartService;
            this.payPalService = payPalService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var orders = await ordersService.GetAllAsync();
            return View(orders);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var order = await ordersService.GetByIdAsync(id);
            if (order == null)
                return View("NotFound");

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Administrator") && order.ApplicationUserId != currentUserId)
                return Forbid();

            return View(order);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            var items = cartService.GetItems();
            if (!items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            ViewData["PayPalClientId"] = payPalService.ClientId;
            ViewData["PayPalConfigured"] = payPalService.IsConfigured;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayPalOrder([FromBody] PayPalCheckoutRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = "Enter a valid name and email." });
            if (!payPalService.IsConfigured) return Problem("PayPal is not configured.", statusCode: StatusCodes.Status503ServiceUnavailable);
            var total = cartService.GetItems().Sum(item => item.Price * item.Quantity);
            if (total <= 0) return BadRequest(new { error = "Your cart is empty." });

            try { return Ok(new { id = await payPalService.CreateOrderAsync(total) }); }
            catch { return Problem("Unable to start the PayPal payment.", statusCode: StatusCodes.Status502BadGateway); }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapturePayPalOrder([FromBody] CapturePayPalOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = "Payment details are invalid." });
            if (!cartService.GetItems().Any()) return BadRequest(new { error = "Your cart is empty." });

            try
            {
                var payment = await payPalService.CaptureOrderAsync(request.PayPalOrderId);
                if (!payment.Completed) return BadRequest(new { error = "PayPal did not complete the payment." });
                var order = await CreateOrderFromCart(request.CustomerName, request.CustomerEmail);
                return Ok(new { orderId = order.Id, captureId = payment.CaptureId });
            }
            catch { return Problem("Unable to capture the PayPal payment.", statusCode: StatusCodes.Status502BadGateway); }
        }

        [HttpPost]
        [Authorize]
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

            var order = await CreateOrderFromCart(customerName, customerEmail);

            TempData["StatusMessage"] = "Order placed successfully.";
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        private async Task<Order> CreateOrderFromCart(string customerName, string customerEmail)
        {
            var cartItems = cartService.GetItems();
            var order = new Order { CustomerName = customerName, CustomerEmail = customerEmail, ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier), OrderDate = DateTime.UtcNow, TotalPrice = cartItems.Sum(i => i.Price * i.Quantity), OrderItems = cartItems.Select(i => new OrderItem { MovieId = i.MovieId, Quantity = i.Quantity, UnitPrice = i.Price }).ToList() };
            await ordersService.AddAsync(order);
            await ordersService.SaveAsync();
            cartService.ClearCart();
            return order;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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
