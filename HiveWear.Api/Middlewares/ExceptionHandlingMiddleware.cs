using FluentValidation;

namespace HiveWear.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                // Set CORS headers before writing the response
                context.Response.Headers["Access-Control-Allow-Origin"] = "https://red-moss-0cb083103.6.azurestaticapps.net";
                context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
                context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
                context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var response = new { Message = "Validation failed", Errors = errors };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
