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
        private readonly IActorMoviesService actorMoviesService;

        public MoviesController(IMoviesService moviesService,ICinemasService cinemasService,
            IProducersService producersService,IActorsService actorsService,IActorMoviesService actorMoviesService,IMapper mapper)
        {
            this.moviesService = moviesService;
            this.cinemasService = cinemasService;
            this.producersService = producersService;
            this.actorsService = actorsService;
            this.actorMoviesService = actorMoviesService;
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
                    var movie = mapper.Map<Movie>(viewModel);
                    await moviesService.AddAsync(movie);
                    await moviesService.SaveAsync();

                    if (viewModel.SelectedActorIds != null && viewModel.SelectedActorIds.Any())
                    {
                        foreach (var actorId in viewModel.SelectedActorIds)
                        {
                            await actorMoviesService.AddAsync(new Actor_Movie
                            {
                                MovieId = movie.Id,
                                ActorId = actorId
                            });
                        }
                        await actorMoviesService.SaveAsync();
                    }

                    TempData["SuccessMessage"] = $"{movie.Name} was added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Something went wrong while adding the movie. Please try again.");
                }
            }

            viewModel.Cinemas = await cinemasService.GetAllAsync();
            viewModel.Producers = await producersService.GetAllAsync();
            viewModel.Actors = await actorsService.GetAllAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await moviesService.GetByIdAsync(id);
            if (movie == null)
            {
                return View("NotFound");
            }
            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var allMovies = await moviesService.GetAllAsync();

            var results = string.IsNullOrWhiteSpace(query)
                ? new List<Movie>()
                : allMovies.Where(m => m.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewData["SearchQuery"] = query;
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await moviesService.GetByIdAsync(id);
            if (movie == null)
                return View("NotFound");

            var viewModel = mapper.Map<EditMovieViewModel>(movie);

            viewModel.Cinemas = await cinemasService.GetAllAsync();
            viewModel.Producers = await producersService.GetAllAsync();
            viewModel.Actors = await actorsService.GetAllAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditMovieViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var movie = mapper.Map<Movie>(viewModel);
                    await moviesService.UpdateAsync(id, movie);
                    await moviesService.SaveAsync();

                    await actorMoviesService.DeleteByMovieIdAsync(id);
                    if (viewModel.SelectedActorIds != null && viewModel.SelectedActorIds.Any())
                    {
                        foreach (var actorId in viewModel.SelectedActorIds)
                        {
                            await actorMoviesService.AddAsync(new Actor_Movie
                            {
                                MovieId = id,
                                ActorId = actorId
                            });
                        }
                    }
                    await actorMoviesService.SaveAsync();

                    TempData["SuccessMessage"] = "Movie updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the movie: {ex.Message}");
                }
            }

            viewModel.Cinemas = await cinemasService.GetAllAsync();
            viewModel.Producers = await producersService.GetAllAsync();
            viewModel.Actors = await actorsService.GetAllAsync();
            return View(viewModel);
        }
    }
}
