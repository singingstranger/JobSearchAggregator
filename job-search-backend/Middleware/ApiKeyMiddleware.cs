public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _validKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _validKey = config["API_KEY"] ?? throw new InvalidOperationException("API_KEY not configured");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-API-Key", out var apiKey) || apiKey != _validKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(context);
    }
}