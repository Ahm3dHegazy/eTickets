using AutoMapper;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorsService actorsService;
        private readonly IMapper mapper;

        public ActorsController(IActorsService actorsService, IMapper mapper)
        {
            this.actorsService = actorsService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var actors = await actorsService.GetAllAsync();
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
                    Actor actor = mapper.Map<Actor>(viewModel);

                    await actorsService.AddAsync(actor);

                    await actorsService.SaveAsync();

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

        public async Task<IActionResult> Details(int id)
        {
            var actor = await actorsService.GetByIdAsync(id);

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await actorsService.GetByIdAsync(id);
            if (actor == null)
                return NotFound();

            var viewModel = mapper.Map<EditActorViewModel>(actor);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditActorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var actor = mapper.Map<Actor>(viewModel);

                    await actorsService.UpdateAsync(id, actor);
                    await actorsService.SaveAsync();

                    return RedirectToAction(nameof(Index));
                    
                }
                catch (Exception ex)
                {
                   ModelState.AddModelError("", $"Something went wrong while updating the actor: {ex.Message}");
                }
            }
            return View(viewModel);
        }
    }

}
