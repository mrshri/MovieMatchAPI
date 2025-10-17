using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSuggestion.Application.Services;
using MovieSuggestion.Domain.Entities;

namespace MovieSuggestion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
                _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies  =  await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null) {
                return NotFound("No Records Available!");
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
             await _movieService.AddMovieAsync(movie);

            return CreatedAtAction(nameof(GetMovieById), new {id=movie.Id},movie);
        }
    }
}
