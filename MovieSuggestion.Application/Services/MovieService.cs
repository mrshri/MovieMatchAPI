using AutoMapper;
using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Domain.Entities;
using MovieSuggestion.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSuggestion.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repo;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository movieRepository,IMapper mapper)
        {
            _repo = movieRepository;
            _mapper = mapper;
        }
        public async Task AddMovieAsync(MovieCreateDTO movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            await _repo.AddAsync(movie);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieDTO>> GetAllMoviesAsync()
        {
            var movies =  await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<MovieDTO>>(movies);
        }

        public async Task<MovieDTO?> GetMovieByIdAsync(int id)
        {
           var movie = await _repo.GetByIdAsync(id);
            return _mapper.Map<MovieDTO?>(movie);
        }
    }
}
