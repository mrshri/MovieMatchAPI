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

        public MovieService(IMovieRepository movieRepository)
        {
            _repo = movieRepository;
        }
        public async Task AddMovieAsync(Movie movie)
        {
            await _repo.AddAsync(movie);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
           return await _repo.GetByIdAsync(id);
        }
    }
}
