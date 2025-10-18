using FluentValidation;
using MovieSuggestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSuggestion.Application.Validators
{
    public class MovieDTOValidator : AbstractValidator<MovieDTO>
    {
        public MovieDTOValidator()
        {
            RuleFor(movie => movie.Title)
                .NotEmpty().WithMessage("Tile is required.")
                .Length(2, 100).WithMessage("Title must be between 2 and 100 characters.");

            RuleFor(movie => movie.Genre)
                .NotEmpty().WithMessage("Genre is required.")
                .Length(2, 50).WithMessage("Genre must be between 2 and 50 characters.");
        }
    }
}
