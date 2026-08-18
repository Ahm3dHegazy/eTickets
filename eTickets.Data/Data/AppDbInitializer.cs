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

            // Existing installations receive the additional catalog without duplicating records.
            if (context.Cinemas.Any())
            {
                SeedAdditionalCatalog(context);
                return;
            }

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

            SeedAdditionalCatalog(context);
        }

        private static void SeedAdditionalCatalog(AppDbContext context)
        {
            var cinemas = new List<Cinema>
            {
                new() { Name = "Aurora Screens", Logo = "/images/cinemas/cineplex-one.jpeg", Description = "Premium screens with immersive sound." },
                new() { Name = "Harbor Lights Cinema", Logo = "/images/cinemas/grand-cinema.jpeg", Description = "A waterfront destination for film lovers." },
                new() { Name = "Metro Reel", Logo = "/images/cinemas/indie-theater.jpeg", Description = "The latest releases in the city center." },
                new() { Name = "Starlight Multiplex", Logo = "/images/cinemas/movie-palace.jpeg", Description = "Family-friendly entertainment under one roof." },
                new() { Name = "Nova Cinema", Logo = "/images/cinemas/cinema-world.jpeg", Description = "Big stories on spectacular screens." },
                new() { Name = "Riverside Movies", Logo = "/images/cinemas/cineplex-one.jpeg", Description = "Comfortable movie nights by the river." },
                new() { Name = "Orion Theater", Logo = "/images/cinemas/grand-cinema.jpeg", Description = "Classic charm and modern projection." },
                new() { Name = "Panorama Pictures", Logo = "/images/cinemas/indie-theater.jpeg", Description = "Curated films and independent premieres." },
                new() { Name = "Galaxy Grand", Logo = "/images/cinemas/movie-palace.jpeg", Description = "Blockbuster experiences for every audience." },
                new() { Name = "Sunset Cinema", Logo = "/images/cinemas/cinema-world.jpeg", Description = "An easygoing neighborhood theater." }
            };
            AddMissing(context.Cinemas, cinemas, cinema => cinema.Name);

            var producers = new List<Producer>
            {
                new() { FullName = "Olivia Bennett", ProfilePictureURL = "/images/producers/producer-alpha.jpeg", Bio = "Producer of character-driven features." },
                new() { FullName = "Marcus Reed", ProfilePictureURL = "/images/producers/producer-beta.jpeg", Bio = "Producer specializing in action films." },
                new() { FullName = "Sofia Laurent", ProfilePictureURL = "/images/producers/producer-gamma.jpeg", Bio = "Independent film producer and storyteller." },
                new() { FullName = "Daniel Okafor", ProfilePictureURL = "/images/producers/producer-delta.jpeg", Bio = "Documentary producer focused on global stories." },
                new() { FullName = "Hannah Cole", ProfilePictureURL = "/images/producers/producer-epsilon.jpeg", Bio = "Producer of animated family adventures." },
                new() { FullName = "Ethan Park", ProfilePictureURL = "/images/producers/producer-alpha.jpeg", Bio = "Producer of high-concept thrillers." },
                new() { FullName = "Maya Ibrahim", ProfilePictureURL = "/images/producers/producer-beta.jpeg", Bio = "Producer supporting emerging voices." },
                new() { FullName = "Lucas Martin", ProfilePictureURL = "/images/producers/producer-gamma.jpeg", Bio = "Producer of acclaimed drama films." },
                new() { FullName = "Grace Chen", ProfilePictureURL = "/images/producers/producer-delta.jpeg", Bio = "Producer of insightful documentaries." },
                new() { FullName = "Noah Williams", ProfilePictureURL = "/images/producers/producer-epsilon.jpeg", Bio = "Producer of crowd-pleasing comedies." }
            };
            AddMissing(context.Producers, producers, producer => producer.FullName);

            var actors = new List<Actor>
            {
                new() { FullName = "Ava Thompson", ProfilePictureURL = "/images/actors/john-doe.jpeg", Bio = "Versatile dramatic performer." },
                new() { FullName = "Liam Carter", ProfilePictureURL = "/images/actors/jane-smith.jpeg", Bio = "Leading actor known for action roles." },
                new() { FullName = "Mia Rodriguez", ProfilePictureURL = "/images/actors/sam-green.jpeg", Bio = "Award-winning screen actor." },
                new() { FullName = "Ethan Brooks", ProfilePictureURL = "/images/actors/emily-white.jpeg", Bio = "Comedic actor with sharp timing." },
                new() { FullName = "Zoe Mitchell", ProfilePictureURL = "/images/actors/michael-brown.jpeg", Bio = "Actor and voice performer." },
                new() { FullName = "Caleb Foster", ProfilePictureURL = "/images/actors/john-doe.jpeg", Bio = "Rising talent in independent cinema." },
                new() { FullName = "Nora Hayes", ProfilePictureURL = "/images/actors/jane-smith.jpeg", Bio = "Actor with a passion for documentaries." },
                new() { FullName = "Leo Grant", ProfilePictureURL = "/images/actors/sam-green.jpeg", Bio = "Energetic performer and stunt actor." },
                new() { FullName = "Isla Morgan", ProfilePictureURL = "/images/actors/emily-white.jpeg", Bio = "Actor celebrated for heartfelt roles." },
                new() { FullName = "Owen Price", ProfilePictureURL = "/images/actors/michael-brown.jpeg", Bio = "Experienced actor across film and theater." }
            };
            AddMissing(context.Actors, actors, actor => actor.FullName);
            context.SaveChanges();

            var cinemaByName = context.Cinemas.ToDictionary(cinema => cinema.Name);
            var producerByName = context.Producers.ToDictionary(producer => producer.FullName);
            var startDate = DateTime.UtcNow.Date;
            var movies = new List<Movie>
            {
                new() { Name = "Midnight Runway", Description = "A courier races through a sleepless city.", Price = 16m, ImageURL = "/images/movies/action-blast.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Action, CinemaId = cinemaByName["Aurora Screens"].Id, ProducerId = producerByName["Marcus Reed"].Id },
                new() { Name = "The Last Laugh", Description = "Two rivals discover an unlikely friendship.", Price = 12m, ImageURL = "/images/movies/funny-times.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Comedy, CinemaId = cinemaByName["Harbor Lights Cinema"].Id, ProducerId = producerByName["Noah Williams"].Id },
                new() { Name = "Letters From Home", Description = "A family reconnects through a box of letters.", Price = 14m, ImageURL = "/images/movies/dramatic-tales.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Drama, CinemaId = cinemaByName["Metro Reel"].Id, ProducerId = producerByName["Lucas Martin"].Id },
                new() { Name = "Beyond the Reef", Description = "A journey to protect a disappearing coastline.", Price = 11m, ImageURL = "/images/movies/documentary-insights.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Documentary, CinemaId = cinemaByName["Starlight Multiplex"].Id, ProducerId = producerByName["Grace Chen"].Id },
                new() { Name = "Cloud City Club", Description = "Young inventors build a flying clubhouse.", Price = 10m, ImageURL = "/images/movies/funny-times.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Cartoon, CinemaId = cinemaByName["Nova Cinema"].Id, ProducerId = producerByName["Hannah Cole"].Id },
                new() { Name = "Whispers at Hollow Creek", Description = "A quiet town hides a terrifying secret.", Price = 13m, ImageURL = "/images/movies/dramatic-tales.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Horror, CinemaId = cinemaByName["Riverside Movies"].Id, ProducerId = producerByName["Ethan Park"].Id },
                new() { Name = "Second Sunrise", Description = "A musician begins again in a new city.", Price = 14m, ImageURL = "/images/movies/documentary-insights.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Drama, CinemaId = cinemaByName["Orion Theater"].Id, ProducerId = producerByName["Olivia Bennett"].Id },
                new() { Name = "Frame by Frame", Description = "Artists keep a beloved local cinema alive.", Price = 11m, ImageURL = "/images/movies/action-blast.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Documentary, CinemaId = cinemaByName["Panorama Pictures"].Id, ProducerId = producerByName["Daniel Okafor"].Id },
                new() { Name = "Weekend Detectives", Description = "Three friends turn a small mystery into a big adventure.", Price = 12m, ImageURL = "/images/movies/funny-times.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Comedy, CinemaId = cinemaByName["Galaxy Grand"].Id, ProducerId = producerByName["Maya Ibrahim"].Id },
                new() { Name = "Ember Protocol", Description = "An analyst uncovers a dangerous conspiracy.", Price = 16m, ImageURL = "/images/movies/action-blast.jpeg", StartDate = startDate, EndDate = startDate.AddMonths(1), MovieCategory = MovieCategory.Action, CinemaId = cinemaByName["Sunset Cinema"].Id, ProducerId = producerByName["Sofia Laurent"].Id }
            };
            AddMissing(context.Movies, movies, movie => movie.Name);
            context.SaveChanges();

            var actorByName = context.Actors.ToDictionary(actor => actor.FullName);
            var movieByName = context.Movies.ToDictionary(movie => movie.Name);
            foreach (var (actorName, movieName) in new[]
            {
                ("Ava Thompson", "Midnight Runway"), ("Liam Carter", "The Last Laugh"), ("Mia Rodriguez", "Letters From Home"),
                ("Ethan Brooks", "Beyond the Reef"), ("Zoe Mitchell", "Cloud City Club"), ("Caleb Foster", "Whispers at Hollow Creek"),
                ("Nora Hayes", "Second Sunrise"), ("Leo Grant", "Frame by Frame"), ("Isla Morgan", "Weekend Detectives"), ("Owen Price", "Ember Protocol")
            })
            {
                var actorId = actorByName[actorName].Id;
                var movieId = movieByName[movieName].Id;
                if (!context.Actor_Movies.Any(item => item.ActorId == actorId && item.MovieId == movieId))
                    context.Actor_Movies.Add(new Actor_Movie { ActorId = actorId, MovieId = movieId });
            }
            context.SaveChanges();
        }

        private static void AddMissing<TEntity>(DbSet<TEntity> set, IEnumerable<TEntity> items, Func<TEntity, string> keySelector)
            where TEntity : class
        {
            var existingKeys = set.AsNoTracking().AsEnumerable().Select(keySelector).ToHashSet(StringComparer.OrdinalIgnoreCase);
            set.AddRange(items.Where(item => !existingKeys.Contains(keySelector(item))));
        }
    }
}
