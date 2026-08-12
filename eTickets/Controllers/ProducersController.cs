using AutoMapper;
using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class ProducersController : Controller
    {
        private readonly IProducersService producersService;
        private readonly IMapper mapper;

        public ProducersController(IProducersService producersService,IMapper mapper )
        {
            this.producersService = producersService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var producers = await producersService.GetAllAsync();
            return View(producers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await producersService.GetByIdAsync(id);

            if (producerDetails == null) 
                return View("NotFound");

            return View(producerDetails);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateProducerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Producer producer = mapper.Map<Producer>(viewModel);

                    await producersService.AddAsync(producer);

                    await producersService.SaveAsync();

                    TempData["SuccessMessage"] = $"{producer.FullName} added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Something went wrong while adding the producer. Please try again.");
                    return View(viewModel);
                }

            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producer = await producersService.GetByIdAsync(id);
            if (producer == null)
                return View("NotFound");

            var viewModel = mapper.Map<EditProducerViewModel>(producer);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditProducerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var producer = mapper.Map<Producer>(viewModel);

                    await producersService.UpdateAsync(id, producer);
                    await producersService.SaveAsync();

                    TempData["SuccessMessage"] = $"Producer updated successfully.";
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the producer: {ex.Message}");
                }
            }
            return View(viewModel);
        }

    }
}
