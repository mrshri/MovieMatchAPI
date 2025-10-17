using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSuggestion.Application.DTOs;
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
                return NotFound(new { Message = $"Movie with ID {id} not found" });
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody]MovieCreateDTO movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             await _movieService.AddMovieAsync(movieDto);

            return Ok(new { Message = "Movie added successfully!" });
        }
    }
}
