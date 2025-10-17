using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSuggestion.Application.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
            => new ApiResponse<T> { Success = true, Message = message, Data = data };

        public static ApiResponse<T> FailureResponse(string message)
            => new ApiResponse<T> { Success = false, Message = message };
    }
}
