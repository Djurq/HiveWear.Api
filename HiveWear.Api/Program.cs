using HiveWear.Api.Middlewares;
using HiveWear.Application.Extensions;
using HiveWear.Infrastructure.Extensions;
using Serilog;

try
{
/*    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

    Log.Information("Starting up the app...");*/

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
            builder.WithOrigins("https://red-moss-0cb083103.6.azurestaticapps.net")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
    });

    builder.Host.UseSerilog();

    WebApplication app = builder.Build();

    app.UseCors("AllowAllOrigins");

    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
finally
{
    Log.CloseAndFlush();
    Log.Information("Shutting down the app...");
}