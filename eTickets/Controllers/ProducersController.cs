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
        public async Task<IActionResult> Index()
        {
            var producers = await producersService.GetAllAsync();
            return View(producers);
        }
    }
}
