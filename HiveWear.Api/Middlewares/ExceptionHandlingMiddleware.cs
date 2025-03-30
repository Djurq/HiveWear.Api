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
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var response = new { Message = "Validation failed", Errors = errors };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
