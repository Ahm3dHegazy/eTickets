using eTickets.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class Cinema : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public string Location { get; set; } = string.Empty;

        // Relationships
        public List<Movie> Movies { get; set; } = [];
    }
}
