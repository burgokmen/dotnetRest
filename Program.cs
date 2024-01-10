
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using WorkintechRestApiDemo.Business;
using WorkintechRestApiDemo.Business.Authentication;
using WorkintechRestApiDemo.Database;
using WorkintechRestApiDemo.Middleware;

namespace WorkintechRestApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<WorkintechDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("WorkintechRestApiDemoContext"));
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                    };
                });
            // Add services to the container.

            builder.Services.AddControllers();

            //Autofac

            builder.Services.AddScoped<ICurrencyService, CurrencyService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserService, UserService>();

            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.Use(async (ctx, next) =>
            {
                var startTimestamp = DateTime.Now;
                await next();
           
                var elapsed = DateTime.Now - startTimestamp;
                app.Logger.LogWarning($"Request {ctx.Request.Method} {ctx.Request.Path} processed in {elapsed.TotalMilliseconds} ms");

            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseCustomExceptionHandler();

            app.Run();
        }
    }
}
