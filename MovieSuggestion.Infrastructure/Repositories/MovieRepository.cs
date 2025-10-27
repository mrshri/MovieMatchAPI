using Microsoft.EntityFrameworkCore;
using MovieSuggestion.Domain.Entities;
using MovieSuggestion.Infrastructure.DATA;
using MovieSuggestion.Infrastructure.Repositories.Interfaces;

namespace MovieSuggestion.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        public async Task DeleteAsync(Movie movie)
        {
            var existingMovie = await _context.Movies.FindAsync(movie.Id);
            if (existingMovie != null)
            {
                _context.Movies.Remove(existingMovie);
            }
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
           return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<IEnumerable<Movie>> GetRecommendedMoviesAsync(string genre, string? keyword)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(genre) || !string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m =>
                    (string.IsNullOrEmpty(genre) || m.Genre.ToLower() == genre.ToLower()) ||
                    (string.IsNullOrEmpty(keyword) ||
                     m.Title.ToLower().Contains(keyword.ToLower()) ||
                     m.Description.ToLower().Contains(keyword.ToLower()))
                );
            }


            return await query.Take(10).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            var existingMovie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == movie.Id);
            if (existingMovie != null)
            {
                _context.Entry(movie).State = EntityState.Modified;
            }
        }
    }
}
