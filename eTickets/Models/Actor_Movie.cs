using eTickets.Data.Base;

namespace eTickets.Models
{
    public class Actor_Movie : IEntityBase
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } 

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
