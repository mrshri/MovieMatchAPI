using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSuggestion.Application.DTOs;
using MovieSuggestion.Application.Models;
using MovieSuggestion.Application.Services;

namespace MovieSuggestion.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/movies")]
    [ApiController]
    public class MoviesControllerV2 : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesControllerV2(IMovieService movieService, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound(ApiResponse<string>.FailureResponse($"Movie with ID {id} not found"));

            return Ok(ApiResponse<MovieDTO>.SuccessResponse(movie, "Movie fetched successfully"));
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> PatchMovie(int id,[FromBody] MoviePatchDto movieDto)
        {
            if (id != movieDto.Id)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("ID MisMatch."));
            }
          
            var updatedMovie = await _movieService.PatchMovieAsync(movieDto);
            return Ok(ApiResponse<MovieDTO>.SuccessResponse(updatedMovie, "Movie updated successfully."));
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
