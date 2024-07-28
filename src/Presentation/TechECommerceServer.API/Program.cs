using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;
using TechECommerceServer.API.Configurations.ColumnWriters;
using TechECommerceServer.Application;
using TechECommerceServer.Application.Exceptions;
using TechECommerceServer.Infrastructure;
using TechECommerceServer.Infrastructure.Services.Storage.Local;
using TechECommerceServer.Persistence;
using TechECommerceServer.SignalR;

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

        Logger logger = new LoggerConfiguration()
            .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("Default"), "Logs", needAutoCreateTable: true,
            columnOptions: new Dictionary<string, ColumnWriterBase>
            {
                {"message", new RenderedMessageColumnWriter()},
                {"message_template", new MessageTemplateColumnWriter()},
                {"level", new LevelColumnWriter()},
                {"time_stamp", new TimestampColumnWriter()},
                {"exception", new ExceptionColumnWriter()},
                {"log_event", new LogEventSerializedColumnWriter()},
                {"user_name", new UserNameColumnWriter()}
            })
            .WriteTo.Seq(serverUrl: builder.Configuration.GetValue<string>("Seq:ServerUrl")!)
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .CreateLogger();
        builder.Host.UseSerilog(logger);

        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestHeaders.Add("sec-ch-ua");
            logging.ResponseHeaders.Add("MyResponseHeader");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
            logging.CombineLogs = true;
        });

        IWebHostEnvironment hostEnvironment = builder.Environment;
        builder.Configuration
            .SetBasePath(hostEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true);

        if (hostEnvironment.IsDevelopment())
            builder.Configuration.AddUserSecrets<Program>();
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddPersistenceServices();

        // Add SignalR services (extension) to system.
        builder.Services.AddSignalRServices();

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Token:SecurityKey"]!)),
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires is not null ? expires > DateTime.UtcNow : false,
                    NameClaimType = ClaimTypes.Name // note: can get 'Name' propery from User.Identity.Name property
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

        app.UseSerilogRequestLogging();
        app.UseHttpLogging();

        app.UseCors();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            string? userName = context.User?.Identity?.IsAuthenticated is not null || true ? context.User.Identity.Name : null;
            LogContext.PushProperty("user_name", userName);
            await next();
        });

        app.MapControllers();
        app.MapHubs();

        app.Run();
    }
}