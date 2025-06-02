using Serilog;
using System.Diagnostics;
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
            var sw = Stopwatch.StartNew();
            var request = context.Request;
            var originalBody = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            sw.Stop();
            memoryStream.Seek(0, SeekOrigin.Begin);

            Log.Information("HTTP {Method} {Path} → {StatusCode} in {Elapsed}ms",
                request.Method,
                request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds);

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBody);
        }
    }
}
