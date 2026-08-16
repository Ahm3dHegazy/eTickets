using System.ComponentModel.DataAnnotations;

namespace eTickets.web.ViewModels
{
    public class CreateActorViewModel
    {
        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters."), MinLength(3, ErrorMessage = "Full name must be at least 3 characters long.")]
        public string FullName { get; set; }

        [Display(Name = "Profile Picture URL")]
        [Url(ErrorMessage = "Please enter a valid image URL (e.g. https://example.com/photo.jpeg)")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "Biography")]
        [Length(0, 1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
        public string Bio { get; set; }
    }
}
