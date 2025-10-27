using System.ComponentModel.DataAnnotations;

namespace MovieSuggestion.Application.DTOs
{
    public class MovieRecommendationRequestDto
    {
        [Required(ErrorMessage = "Genre is required.")]
        public string Genre { get; set; } = string.Empty;

        public string? Keyword { get; set; }
    }
}
