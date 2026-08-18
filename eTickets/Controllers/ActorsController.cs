using AutoMapper;
using eTickets.Business.Interfaces;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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

                    TempData["SuccessMessage"] = $"{actor.FullName} was added successfully.";
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
                return View("NotFound");

            return View(actor);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await actorsService.GetByIdAsync(id);
            if (actor == null)
                return View("NotFound");

            var viewModel = mapper.Map<EditActorViewModel>(actor);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditActorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var actor = mapper.Map<Actor>(viewModel);

                    await actorsService.UpdateAsync(id, actor);
                    await actorsService.SaveAsync();

                    TempData["SuccessMessage"] = $"Actor updated successfully.";
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the actor: {ex.Message}");
                }
            }
            return View(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await actorsService.GetByIdAsync(id);
            if (actor == null)
                return View("NotFound");

            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var actor = await actorsService.GetByIdAsync(id);
                var name = actor?.FullName ?? "Actor";

                await actorsService.DeleteAsync(id);
                await actorsService.SaveAsync();

                TempData["SuccessMessage"] = $"{name} was deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the actor: {ex.Message}");
                var actor = await actorsService.GetByIdAsync(id);
                return View("Delete", actor);
            }
        }
    }

}
