using AutoMapper;
using eTickets.Models;
using eTickets.web.ViewModels;

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

            CreateMap<EditProducerViewModel, Producer>();
            CreateMap<Producer, EditProducerViewModel>();

            CreateMap<CreateCinemaViewModel, Cinema>();

            CreateMap<EditCinemaViewModel, Cinema>();
            CreateMap<Cinema, EditCinemaViewModel>();

            CreateMap<CreateMovieViewModel, Movie>()
       .ForMember(dest => dest.Actor_Movies, opt => opt.Ignore())
       .ForMember(dest => dest.Cinema, opt => opt.Ignore())
       .ForMember(dest => dest.Producer, opt => opt.Ignore());


            CreateMap<Movie, EditMovieViewModel>()
                .ForMember(dest => dest.SelectedActorIds,
                    opt => opt.MapFrom(src => src.Actor_Movies.Select(am => am.ActorId).ToList()));

            CreateMap<EditMovieViewModel, Movie>()
                .ForMember(dest => dest.Actor_Movies, opt => opt.Ignore())
                .ForMember(dest => dest.Cinema, opt => opt.Ignore())
                .ForMember(dest => dest.Producer, opt => opt.Ignore());
        }
    }
}
