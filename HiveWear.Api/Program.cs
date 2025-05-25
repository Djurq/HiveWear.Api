using HiveWear.Api.Middlewares;
using HiveWear.Application.Extensions;
using HiveWear.Infrastructure.Database;
using HiveWear.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
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
            builder.WithOrigins("http://localhost:8080") // Allow localhost:8080
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
    });

    builder.Host.UseSerilog();

    WebApplication app = builder.Build();


/*    using (IServiceScope scope = app.Services.CreateScope())
    {
        HiveWearDbContext db = scope.ServiceProvider.GetRequiredService<HiveWearDbContext>();
        db.Database.Migrate();
    }*/

    app.UseCors("AllowAllOrigins");

    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

/*    app.UseHttpsRedirection();*/

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