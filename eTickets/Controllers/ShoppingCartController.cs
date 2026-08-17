using Microsoft.AspNetCore.Mvc;
using eTickets.Business.Interfaces;

namespace eTickets.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService cartService;
        private readonly IMoviesService moviesService;

        public ShoppingCartController(IShoppingCartService cartService, IMoviesService moviesService)
        {
            this.cartService = cartService;
            this.moviesService = moviesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var items = cartService.GetItems();
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int movieId, int quantity = 1)
        {
            var movie = await moviesService.GetByIdAsync(movieId);
            if (movie == null)
                return NotFound();

            cartService.AddToCart(movie.Id, movie.Name, movie.ImageURL, movie.Price, quantity);

            //TempData["StatusMessage"] = $"{movie.Name} added to cart.";
            return RedirectToAction("Index", "Movies");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int movieId, int quantity)
        {
            var current = cartService.GetItems().FirstOrDefault(i => i.MovieId == movieId);
            if (current != null)
            {
                var diff = quantity - current.Quantity;
                if (diff > 0)
                    cartService.AddToCart(movieId, current.MovieName, current.MovieImageURL, current.Price, diff);
                else if (diff < 0)
                    cartService.RemoveFromCart(movieId, -diff);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int movieId)
        {
            var item = cartService.GetItems().FirstOrDefault(i => i.MovieId == movieId);
            if (item != null)
                cartService.RemoveFromCart(movieId, item.Quantity);

            //TempData["StatusMessage"] = "Item removed from cart.";
            return RedirectToAction(nameof(Index));
        }
    }
}