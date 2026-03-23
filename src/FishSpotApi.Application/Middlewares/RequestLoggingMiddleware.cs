using LoggerFactory = FishSpotApi.Logger.LoggerFactory;

namespace FishSpotApi.Application.Middlewares
{
    public class RequestLoggingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            LoggerFactory.Info($"Handling request: {context.Request.Method} - {context.Request.Path}");

            await _next(context);

            LoggerFactory.Info($"Finished handling request. Status Code: {context.Response.StatusCode}");
        }
    }
}