using AutoMapper;
using MovieAPIDemo.Server.Entities;
using MovieAPIDemo.Server.Models;

namespace MovieAPIDemo.Server
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Movie, MovieListViewModel>( );
            CreateMap<Movie, MovieDetailsViewModel>( );
            CreateMap<MovieListViewModel, Movie>( );
            CreateMap<CreateMovieViewModel, Movie>( ).ForMember( x =>  x.Actors, y => y.Ignore( ) );

            CreateMap<Person, ActorViewModel>( );
            CreateMap<Person, ActorDetailsViewModel>( );
            CreateMap<ActorViewModel, Person>( );
        }
    }
}
