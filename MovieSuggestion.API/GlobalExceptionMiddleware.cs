using Serilog;
using System.Net;

namespace MovieSuggestion.API
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate requestDelegate)
        {
                _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    success = false,
                    message = ex.Message,
                    details = app.EnvironmentName(context)
                    
                };

            }
        }
        public static class app
        {
            public static string EnvironmentName(HttpContext context)
            {
                return context.RequestServices.GetService<IHostEnvironment>()?.EnvironmentName ?? "Unknown";
            }
        }
    }
}
