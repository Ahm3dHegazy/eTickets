using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels
{
    public class CreateActorViewModel
    {
        public string FullName { get; set; }

        [Display(Name = "Profile Picture URL")]
        [Url(ErrorMessage = "Please enter a valid image URL (e.g. https://example.com/photo.jpeg)")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "Biography")]
        [Length(0, 1000, ErrorMessage = "Bio cannot exceed 1000 characters.")]
        public string Bio { get; set; }
    }
}
