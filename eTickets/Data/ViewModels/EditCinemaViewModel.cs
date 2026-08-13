using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels
{
    public class EditCinemaViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [RegularExpression(@"^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|svg))$", ErrorMessage = "Please enter a valid image URL (e.g. https://example.com/logo.png)")]
        public string Logo { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }
    }
}
