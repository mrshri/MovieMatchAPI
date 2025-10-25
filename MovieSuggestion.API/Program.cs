using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieSuggestion.API.Filter;
using MovieSuggestion.Application;
using MovieSuggestion.Application.Services;
using MovieSuggestion.Infrastructure.DATA;
using MovieSuggestion.Infrastructure.Repositories;
using MovieSuggestion.Infrastructure.Repositories.Interfaces;
using Serilog;

namespace MovieSuggestion.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/moviesuggestion_log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddControllers(Options =>
            {
                Options.Filters.Add<ValidationFilter>(); //VALIDATION FILTER added globally
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MovieSuggestion API",
                    Version = "v1",
                    Description = "An ASP.NET Core Web API for suggesting movies based on user preferences.",
                });
            });

            //DATABASE Connection
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //add services & repositories
            builder.Services.AddScoped<IMovieService, MovieService>();

           builder.Services.AddScoped<IMovieRepository,MovieRepository>();

            //configure AutoMapper
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());     

            var app = builder.Build();

            // Adding Global Exception Middleware
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            try
            {
                Log.Information("Starting MovieSuggestion  API...");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "MovieSuggestion API terminated unexpectedly!");

            }
        }
    }
}
