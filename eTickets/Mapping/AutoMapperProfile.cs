using AutoMapper;
using eTickets.Models;

namespace eTickets.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateActorViewModel, Actor>();
            CreateMap<EditActorViewModel, Actor>();
            CreateMap<Actor, EditActorViewModel>();
            CreateMap<CreateProducerViewModel, Producer>();
        }
    }
}
