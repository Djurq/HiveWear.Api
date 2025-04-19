using System.Text;

namespace HiveWear.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestBody = await ReadStreamAsync(context.Request.Body);
            context.Request.Body.Position = 0; // Reset stream na lezen

            _logger.LogInformation("Incoming Request: {method} {url} \nHeaders: {headers}\nBody: {body}",
                context.Request.Method,
                context.Request.Path,
                context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                requestBody);

            var originalResponseBody = context.Response.Body;
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            await _next(context); // Verwerk de request

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("Response Status: {statusCode}\nBody: {body}",
                context.Response.StatusCode,
                responseBody);

            // Zet de response terug naar de echte output stream
            await newResponseBody.CopyToAsync(originalResponseBody);
        }

        private async Task<string> ReadStreamAsync(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            return await reader.ReadToEndAsync();
        }
    }


}
