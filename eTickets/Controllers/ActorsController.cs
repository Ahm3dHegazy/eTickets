using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsService actorsService;

        public ActorsController(IActorsService actorsService)
        {
            this.actorsService = actorsService;
        }
        public async Task<IActionResult> Index()
        {
            var actors = await actorsService.GetAll();
            return View(actors);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
