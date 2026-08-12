using AutoMapper;
using eTickets.Data;
using eTickets.Data.Services;
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
    }
}
