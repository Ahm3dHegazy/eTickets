 using eTickets.Models;
using eTickets.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eTickets.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Apply migrations (if using migrations)
            // context.Database.Migrate();

            // Exit if data already seeded
            if (context.Cinemas.Any()) return;

            // Seed Cinemas (logos reference files under wwwroot/images/cinemas/)
            var cinemas = new List<Cinema>
            {
                new Cinema { Name = "Cineplex One",   Logo = "/images/cinemas/cineplex-one.jpeg", Description = "Modern cinema in downtown." },
                new Cinema { Name = "Grand Cinema",   Logo = "/images/cinemas/grand-cinema.jpeg", Description = "Large-screen experiences." },
                new Cinema { Name = "Indie Theater",  Logo = "/images/cinemas/indie-theater.jpeg", Description = "Independent films and classics." },
                new Cinema { Name = "Movie Palace",   Logo = "/images/cinemas/movie-palace.jpeg", Description = "Luxury cinema experience." },
                new Cinema { Name = "Cinema World",   Logo = "/images/cinemas/cinema-world.jpeg", Description = "World-class cinema." },
            };
            context.Cinemas.AddRange(cinemas);

            // Seed Producers (profile pictures under wwwroot/images/producers/)
            var producers = new List<Producer>
            {
                new Producer { FullName = "Producer Alpha",   ProfilePictureURL = "/images/producers/producer-alpha.jpeg",   Bio = "Experienced producer." },
                new Producer { FullName = "Producer Beta",    ProfilePictureURL = "/images/producers/producer-beta.jpeg",    Bio = "Indie producer." },
                new Producer { FullName = "Producer Gamma",   ProfilePictureURL = "/images/producers/producer-gamma.jpeg",   Bio = "Award-winning producer." },
                new Producer { FullName = "Producer Delta",   ProfilePictureURL = "/images/producers/producer-delta.jpeg",   Bio = "Documentary producer." },
                new Producer { FullName = "Producer Epsilon", ProfilePictureURL = "/images/producers/producer-epsilon.jpeg", Bio = "Experimental producer." }
            };
            context.Producers.AddRange(producers);

            // Seed Actors (profile pictures under wwwroot/images/actors/)
            var actors = new List<Actor>
            {
                new Actor { FullName = "John Doe",      ProfilePictureURL = "/images/actors/john-doe.jpeg",      Bio = "Lead actor." },
                new Actor { FullName = "Jane Smith",    ProfilePictureURL = "/images/actors/jane-smith.jpeg",    Bio = "Supporting actor." },
                new Actor { FullName = "Sam Green",     ProfilePictureURL = "/images/actors/sam-green.jpeg",     Bio = "Character actor." },
                new Actor { FullName = "Emily White",   ProfilePictureURL = "/images/actors/emily-white.jpeg",   Bio = "Rising star." },
                new Actor { FullName = "Michael Brown", ProfilePictureURL = "/images/actors/michael-brown.jpeg", Bio = "Veteran actor." }
            };
            context.Actors.AddRange(actors);

            context.SaveChanges();

            // Seed Movies (movie images under wwwroot/images/movies/)
            var movies = new List<Movie>
            {
                new Movie
                {
                    Name = "Action Blast",
                    Description = "High octane action movie.",
                    Price = 15.00m,
                    ImageURL = "/images/movies/action-blast.jpeg",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    MovieCategory = MovieCategory.Action,
                    CinemaId = cinemas[0].Id,
                    ProducerId = producers[0].Id
                },
                new Movie
                {
                    Name = "Funny Times",
                    Description = "A comedy for the whole family.",
                    Price = 12.50m,
                    ImageURL = "/images/movies/funny-times.jpeg",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    MovieCategory = MovieCategory.Comedy,
                    CinemaId = cinemas[1].Id,
                    ProducerId = producers[1].Id
                },
                new Movie
                {
                    Name = "Dramatic Tales",
                    Description = "A gripping drama.",
                    Price = 14.00m,
                    ImageURL = "/images/movies/dramatic-tales.jpeg",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    MovieCategory = MovieCategory.Drama,
                    CinemaId = cinemas[2].Id,
                    ProducerId = producers[2].Id
                },
                new Movie
                {
                    Name = "Documentary Insights",
                    Description = "An insightful documentary.",
                    Price = 10.00m,
                    ImageURL = "/images/movies/documentary-insights.jpeg",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    MovieCategory = MovieCategory.Documentary,
                    CinemaId = cinemas[3].Id,
                    ProducerId = producers[3].Id
                }
            };
            context.Movies.AddRange(movies);
            context.SaveChanges();

            // Seed Actor_Movie relationships
            var actorMovies = new List<Actor_Movie>
            {
                new Actor_Movie { ActorId = actors[0].Id, MovieId = movies[0].Id },
                new Actor_Movie { ActorId = actors[1].Id, MovieId = movies[0].Id },
                new Actor_Movie { ActorId = actors[1].Id, MovieId = movies[1].Id },
                new Actor_Movie { ActorId = actors[2].Id, MovieId = movies[1].Id },
                new Actor_Movie { ActorId = actors[2].Id, MovieId = movies[2].Id },
            };
            context.AddRange(actorMovies);
            context.SaveChanges();
        }
    }
}