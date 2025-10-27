using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Application.Models;
using MovieSuggestion.Application.Services;

namespace MovieSuggestion.API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieService movieService, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {

            var movies = await _movieService.GetAllMoviesAsync();
            _logger.LogInformation("Fetched {Count} movies", movies.Count());
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
        [Authorize]
        public async Task<IActionResult> AddMovie([FromBody] MovieCreateDTO movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Validation failed"));

            }
            await _movieService.AddMovieAsync(movieDto);

            return Ok(ApiResponse<string>.SuccessResponse("Movie added successfully"));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateMovie([FromBody] MovieUpdateDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Validation failed"));
            }
            await _movieService.UpdateMovieAsync(movieDto);
            return Ok(ApiResponse<string>.SuccessResponse("Movie updated successfully"));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteMovie([FromBody] MovieDeleteDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Validation failed"));
            }
            await _movieService.DeleteMovieAsync(movieDto);
            return Ok(ApiResponse<string>.SuccessResponse("Movie deleted successfully"));
        }

        [HttpPost("recommend")]
        [Authorize]
        public async Task<IActionResult> GetMovieRecommendations([FromBody] MovieRecommendationRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Validation failed"));
            }
            var recommendations = await _movieService.GetRecommendationsAsync(requestDto);
            return Ok(ApiResponse<IEnumerable<MovieRecommendationResponseDto>>.SuccessResponse(recommendations, "Recommendations fetched successfully"));
        }
    }
}
