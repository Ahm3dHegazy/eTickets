using AutoMapper;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService moviesService;
        private readonly IMapper mapper;
        private readonly ICinemasService cinemasService;
        private readonly IProducersService producersService;
        private readonly IActorsService actorsService;

        public MoviesController(IMoviesService moviesService,ICinemasService cinemasService,
            IProducersService producersService,IActorsService actorsService,IMapper mapper)
        {
            this.moviesService = moviesService;
            this.cinemasService = cinemasService;
            this.producersService = producersService;
            this.actorsService = actorsService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await moviesService.GetAllAsync();

            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CreateMovieViewModel model = new CreateMovieViewModel
            {
                Cinemas = await cinemasService.GetAllAsync(),
                Producers = await producersService.GetAllAsync(),
                Actors = await actorsService.GetAllAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateMovieViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Movie movie = mapper.Map<Movie>(viewModel);

                    await moviesService.AddAsync(movie);

                    await moviesService.SaveAsync();

                    TempData["SuccessMessage"] = $"{movie.Name} was added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Something went wrong while adding the movie. Please try again.");
                    return View(viewModel);
                }

            }

            return View(viewModel);
        }
    }
}
