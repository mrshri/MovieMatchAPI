using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSuggestion.Application.DTOs
{
    public class MovieCreateDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, ErrorMessage = "Genre can't exceed 50 characters")]
        public string Genre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters")]
        public string? Description { get; set; }
    }
}
