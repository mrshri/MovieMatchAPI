using MovieSuggestion.Application.DTOs;

namespace MovieSuggestion.Application.Services
{
     public interface IMovieService
     {
        Task<IEnumerable<MovieDTO>> GetAllMoviesAsync();
        Task<MovieDTO?> GetMovieByIdAsync(int id);
        Task AddMovieAsync(MovieCreateDTO movie);
        Task UpdateMovieAsync(MovieUpdateDto movie);
        Task DeleteMovieAsync(MovieDeleteDto movie);
    }
}
