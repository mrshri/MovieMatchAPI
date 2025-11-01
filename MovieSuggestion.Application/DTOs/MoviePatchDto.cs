using System.ComponentModel.DataAnnotations;

namespace MovieSuggestion.Application.DTOs
{
    public class MoviePatchDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(50)]
        public string? Genre { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
