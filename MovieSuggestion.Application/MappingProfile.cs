using AutoMapper;
using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSuggestion.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDTO>();
            CreateMap<MovieCreateDTO, Movie>();
        }
    }
}
