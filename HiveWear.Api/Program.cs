using HiveWear.Api.Middlewares;
using HiveWear.Application.Extensions;
using HiveWear.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Fatal)
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Fatal)
        .WriteTo.AzureTableStorage(
            connectionString: "DefaultEndpointsProtocol=https;AccountName=hivewear;AccountKey=UV/j8XO9Td0+gpI7yQjq08jEF9Eb0ZEg+52rnVUFzr2Dln19HUSt/RANSIBXTafHQ+iGloWkebMT+AStYgKgMw==;EndpointSuffix=core.windows.net",
            storageTableName: "hiveweartablelogs",
            batchPostingLimit: 1,
            period: TimeSpan.FromSeconds(5))
        .CreateLogger();

    builder.Host.UseSerilog();
    builder.Services.AddControllers();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_key_12345dniuqnweqnwlkdnlkcbioyweofawpefnksdjbfpihfpaenrf"))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
            }
        };
    });

    builder.Services.AddAuthorization();

    Log.Information("Starting up the app...");
    Log.Information("Serilog initialized at {Time}", DateTime.UtcNow);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowCredentialsPolicy", builder =>
        {
            builder
                .WithOrigins("https://red-moss-0cb083103.6.azurestaticapps.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });

        options.AddPolicy("DebugAllowLocalhost", builder =>
        {
            builder
                .WithOrigins(
                    "http://localhost:8080",
                    "http://localhost:5204",
                    "https://localhost:7264")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

    WebApplication app = builder.Build();

    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsProduction())
    {
        app.UseCors("AllowCredentialsPolicy");
        app.UseHttpsRedirection();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseCors("DebugAllowLocalhost");
    }

    app.UseAuthentication(); 
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
finally
{
    Log.Information("Shutting down the app...");
    Log.CloseAndFlush();
}