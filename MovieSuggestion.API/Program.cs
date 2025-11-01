using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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


            //Handling API Versioning
            builder.Services.AddApiVersioning(options =>
            {
                // Default API version (if client doesn't specify)
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Assume default version when unspecified
                options.AssumeDefaultVersionWhenUnspecified = true;

                // Report supported versions in response headers
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                // Add version info in Swagger
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var provider = builder.Services.BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = "MovieSuggestion API",
                        Version = description.ApiVersion.ToString(),
                        Description = "An ASP.NET Core Web API for suggesting movies."
                    });
                }
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
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    }
                    );
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
