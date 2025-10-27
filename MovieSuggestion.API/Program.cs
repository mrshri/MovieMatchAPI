using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieSuggestion.API.Filter;
using MovieSuggestion.Application;
using MovieSuggestion.Application.Models;
using MovieSuggestion.Application.Services;
using MovieSuggestion.Infrastructure.DATA;
using MovieSuggestion.Infrastructure.Repositories;
using MovieSuggestion.Infrastructure.Repositories.Interfaces;
using Serilog;
using System.Text;

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
                .WriteTo.File("logs/movieSuggestion_log.txt", rollingInterval: RollingInterval.Day,retainedFileCountLimit:3)
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
                c.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following string as following : `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });
                  c.AddSecurityRequirement(new OpenApiSecurityRequirement
                   {
                     {
                        new OpenApiSecurityScheme
                        {
                           Reference = new OpenApiReference
                           {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                           }
                        },
                         new string[] {}
                     }
                  });
            });


            //DATABASE Connection
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //add services & repositories
            builder.Services.AddScoped<IMovieService, MovieService>();

           builder.Services.AddScoped<IMovieRepository,MovieRepository>();
           builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

            // Add Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["ApiSettings:JwtOptions:Issuer"], // same as AuthAPI
                        ValidAudience = builder.Configuration["ApiSettings:JwtOptions:Audience"], // same as AuthAPI
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:JwtOptions:SecretKey"]))
                    };
                });



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

            app.UseAuthentication();

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
