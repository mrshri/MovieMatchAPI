using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Application.Models;
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
            return Ok(ApiResponse<IEnumerable<MovieDTO>>.SuccessResponse(movies, "Movies fetched successfully"));
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound(ApiResponse<string>.FailureResponse($"Movie with ID {id} not found"));

            return Ok(ApiResponse<MovieDTO>.SuccessResponse(movie, "Movie fetched successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody]MovieCreateDTO movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Validation failed"));

            }
            await _movieService.AddMovieAsync(movieDto);

            return Ok(ApiResponse<string>.SuccessResponse("Movie added successfully"));
        }
    }
}
