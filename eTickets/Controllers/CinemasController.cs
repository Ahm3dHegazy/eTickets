using AutoMapper;
using eTickets.Data;
using eTickets.Data.Services;
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
    }
}
