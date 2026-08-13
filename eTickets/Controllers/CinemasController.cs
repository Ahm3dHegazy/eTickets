using AutoMapper;
using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasService cinemasService;
        private readonly IMapper mapper;

        public CinemasController(ICinemasService cinemasService, IMapper mapper)
        {
            this.cinemasService = cinemasService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var cinemas = await cinemasService.GetAllAsync();

            return View(cinemas);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateCinemaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Cinema cinema = mapper.Map<Cinema>(viewModel);

                    await cinemasService.AddAsync(cinema);

                    await cinemasService.SaveAsync();

                    TempData["SuccessMessage"] = $"{cinema.Name} was added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Something went wrong while adding the cinema. Please try again.");
                    return View(viewModel);
                }

            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cinema = await cinemasService.GetByIdAsync(id);

            if (cinema == null)
                return View("NotFound");

            return View(cinema);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await cinemasService.GetByIdAsync(id);
            if (cinema == null)
                return View("NotFound");

            var viewModel = mapper.Map<EditCinemaViewModel>(cinema);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCinemaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cinema = mapper.Map<Cinema>(viewModel);

                    await cinemasService.UpdateAsync(id, cinema);
                    await cinemasService.SaveAsync();

                    TempData["SuccessMessage"] = $"Cinema updated successfully.";
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the cinema: {ex.Message}");
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await cinemasService.GetByIdAsync(id);
            if (cinema == null)
                return View("NotFound");

            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cinema = await cinemasService.GetByIdAsync(id);
                var name = cinema?.Name ?? "Cinema";

                await cinemasService.DeleteAsync(id);
                await cinemasService.SaveAsync();

                TempData["SuccessMessage"] = $"{name} was deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the cinema: {ex.Message}");
                var cinema = await cinemasService.GetByIdAsync(id);
                return View("Delete", cinema);
            }
        }

    }
}
