
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Watchlist_Tracker.Data;
using Watchlist_Tracker.Models;
using Watchlist_Tracker.Repositories;
using Watchlist_Tracker.Services;
using Watchlist_Tracker.Middleware;

namespace Watchlist_Tracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/application-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7)
                .CreateLogger();

            builder.Host.UseSerilog();
            builder.Logging.ClearProviders();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            var jwtKey = builder.Configuration["Jwt:Key"]!;
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            // Register Repositories
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Register Services
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IFriendshipService, FriendshipService>();
            builder.Services.AddScoped<IWatchlistService, WatchlistService>();

            // Add CORS for frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "http://localhost", "http://frontend")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Add services to the container
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Swagger"));
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.InitializeAsync(services);
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowFrontend");

            app.UseGlobalExceptionHandler();
            app.UseStaticFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
