using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechECommerceServer.Application;
using TechECommerceServer.Application.Exceptions;
using TechECommerceServer.Infrastructure;
using TechECommerceServer.Infrastructure.Services.Storage.Local;
using TechECommerceServer.Persistence;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add custom services to the container.
        builder.Services.AddCors(options => // note: initially, these were the necessary configurations, and the next step is to write a custom http client server on any client app.
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        IWebHostEnvironment hostEnvironment = builder.Environment;
        builder.Configuration
            .SetBasePath(hostEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true);

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddPersistenceServices();

        // Add services for storage system.
        builder.Services.AddStorage<LocalStorage>();

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Admin", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true, // note: the value of the token to be created is the value that we determine who/which origins/sites will use
                    ValidateIssuer = true, // note: is a field indicating who distributed the token value to be created
                    ValidateLifetime = true, // note: is a value that controls the duration of the generated token value
                    ValidateIssuerSigningKey = true, // note: it is the verification of the security key data, which means that the token value to be generated is a value belonging to our application
                    ValidAudience = builder.Configuration["JWT:Token:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Token:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Token:SecurityKey"]))
                };
            });

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configure custom services to pipeline.
        app.ConfigureExceptionHandlingMiddleware();

        // Add services for file system.
        app.UseStaticFiles();

        app.UseCors();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}