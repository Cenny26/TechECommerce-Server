using TechECommerceServer.Application;
using TechECommerceServer.Application.Exceptions;
using TechECommerceServer.Infrastructure;
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

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices();
        builder.Services.AddPersistenceServices();

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configure custom services to pipeline.
        app.ConfigureExceptionHandlingMiddleware();

        app.UseCors();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}