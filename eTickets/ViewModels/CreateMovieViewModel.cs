using eTickets.Data.Enums;
using eTickets.Models;
using System.ComponentModel.DataAnnotations;

namespace eTickets.web.ViewModels
{
    public class CreateMovieViewModel
    {
        [Required(ErrorMessage = "Movie title is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
        [Display(Name = "Title")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Movie Category")]
        public MovieCategory MovieCategory { get; set; }

        [Required(ErrorMessage = "Poster image URL is required.")]
        [RegularExpression(@"^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|svg))$", ErrorMessage = "Must be a valid image URL.")]
        [Display(Name = "Poster")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Please select a cinema.")]
        [Display(Name = "Cinema")]
        public int CinemaId { get; set; }
        public IEnumerable<Cinema> Cinemas { get; set; } = new List<Cinema>();

        [Required(ErrorMessage = "Please select a producer.")]
        [Display(Name = "Producer")]
        public int ProducerId { get; set; }
        public IEnumerable<Producer> Producers { get; set; } = new List<Producer>();

        [Display(Name = "Actors")]
        public List<int> SelectedActorIds { get; set; } = new();
        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();
    }
}
