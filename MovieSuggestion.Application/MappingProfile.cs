using AutoMapper;
using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Domain.Entities;

namespace MovieSuggestion.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDTO>();
            CreateMap<MovieCreateDTO, Movie>();
            CreateMap<MovieUpdateDto, Movie>();
        }
    }
}
