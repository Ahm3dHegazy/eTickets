using System.Security.Claims;
using eTickets.Business.Interfaces;
using eTickets.Data.Models;
using eTickets.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers;

[Authorize]
public class MovieReviewsController : Controller
{
    private readonly IMovieReviewsService reviewsService;
    private readonly IMoviesService moviesService;

    public MovieReviewsController(IMovieReviewsService reviewsService, IMoviesService moviesService)
    {
        this.reviewsService = reviewsService;
        this.moviesService = moviesService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([Bind(Prefix = "ReviewForm")] CreateMovieReviewViewModel model)
    {
        var movie = await moviesService.GetByIdAsync(model.MovieId);
        if (movie == null) return NotFound();

        if (!ModelState.IsValid)
        {
            TempData["ReviewError"] = "Please choose a rating and write a review of at least 5 characters.";
            return RedirectToAction("Details", "Movies", new { id = model.MovieId });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await reviewsService.AddOrUpdateAsync(new MovieReview
        {
            MovieId = model.MovieId,
            ApplicationUserId = userId,
            Rating = model.Rating,
            Comment = model.Comment?.Trim()
        });

        TempData["ReviewSuccess"] = "Your review has been saved.";
        return RedirectToAction("Details", "Movies", new { id = model.MovieId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await reviewsService.GetByIdAsync(id);
        if (review == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!User.IsInRole("Administrator") && review.ApplicationUserId != userId)
            return Forbid();

        var movieId = review.MovieId;
        await reviewsService.DeleteAsync(review);
        TempData["ReviewSuccess"] = "Review deleted.";
        return RedirectToAction("Details", "Movies", new { id = movieId });
    }
}
