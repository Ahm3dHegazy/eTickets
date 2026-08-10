using eTickets.Data.Services;
using eTickets.Models;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateActorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Actor actor = new Actor()
                    {
                        FullName = viewModel.FullName,
                        ProfilePictureURL = viewModel.ProfilePictureURL,
                        Bio = viewModel.Bio
                    };
                    await actorsService.Add(actor);

                    await actorsService.Save();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Something went wrong while adding the actor. Please try again.");
                    return View(viewModel);
                }

            }

            return View(viewModel);
        }
    }
}
