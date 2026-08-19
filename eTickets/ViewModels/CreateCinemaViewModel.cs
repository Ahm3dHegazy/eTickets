using System.ComponentModel.DataAnnotations;

namespace eTickets.web.ViewModels
{
    public class CreateCinemaViewModel
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [RegularExpression(@"^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|svg))$", ErrorMessage = "Logo must be a valid image URL.")]
        public string Logo { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 300 characters.")]
        public string Location { get; set; }
    }
}
