using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;

namespace eTickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMoviesService moviesService;
        private readonly ILogger<HomeController> logger;


        public HomeController(IMoviesService moviesService, ILogger<HomeController> logger)
        {
            this.moviesService = moviesService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await moviesService.GetAllAsync();
            return View(movies.Take(4));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            logger.LogInformation(
                "Contact form submitted by {Name} <{Email}> — Subject: {Subject}",
                model.Name, model.Email, model.Subject);

            TempData["SuccessMessage"] = "Thanks for reaching out! We'll get back to you soon.";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Home/Error/{statusCode:int?}")]
        public IActionResult Error(int? statusCode = null)
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // If we got here via an unhandled exception, log it (details never reach the view)
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature?.Error != null)
            {
                logger.LogError(exceptionFeature.Error,
                    "Unhandled exception on {Path}. RequestId: {RequestId}",
                    exceptionFeature.Path, requestId);
            }

            var model = new ErrorViewModel
            {
                RequestId = requestId,
                StatusCode = statusCode
            };

            Response.StatusCode = statusCode ?? StatusCodes.Status500InternalServerError;

            return View(model);
        }
    }
}
