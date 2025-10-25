
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieSuggestion.AuthApi.Models;
using MovieSuggestion.AuthApi.Service;
using MovieSuggestion.AuthApi.Service.IService;

namespace MovieSuggestion.AuthApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //database connection
            builder.Services.AddDbContext<Data.AuthDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection")));

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddScoped<IJWTTokenGenerator, JWTTokenGenerator>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Data.AuthDbContext>()
               .AddDefaultTokenProviders();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Movie Suggestion - Auth API",
                    Version = "v1"
                });
            });

            var app = builder.Build();

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

            app.Run();
        }
    }
}
