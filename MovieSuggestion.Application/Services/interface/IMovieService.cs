using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSuggestion.Application.Services
{
     public interface IMovieService
     {
        Task<IEnumerable<MovieDTO>> GetAllMoviesAsync();
        Task<MovieDTO?> GetMovieByIdAsync(int id);
        Task AddMovieAsync(MovieCreateDTO movie);
     }
}
