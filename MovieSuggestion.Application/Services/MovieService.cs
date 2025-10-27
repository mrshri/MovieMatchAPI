using AutoMapper;
using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Domain.Entities;
using MovieSuggestion.Infrastructure.Repositories.Interfaces;

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

        public async Task DeleteMovieAsync(MovieDeleteDto movie)
        {
            var movieEntity = await _repo.GetByIdAsync(movie.Id);
            if (movieEntity != null)
            {
                await _repo.DeleteAsync(movieEntity);
                await _repo.SaveChangesAsync();
            }
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

        public async Task UpdateMovieAsync(MovieUpdateDto movie)
        {
            var movieEntity = await _repo.GetByIdAsync(movie.Id);
            if (movieEntity != null)
            {
                _mapper.Map(movie, movieEntity);
                await _repo.UpdateAsync(movieEntity);
                await _repo.SaveChangesAsync();
            }
        }
    }
}
