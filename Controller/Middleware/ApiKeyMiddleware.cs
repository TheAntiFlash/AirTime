namespace Controller.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY = "XAPIKEY";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        if (!context.Request.Headers.TryGetValue(APIKEY, out
                var extractedApiKey)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized Client");
            return;
        }
        var appSettings = context.RequestServices.GetRequiredService < IConfiguration > ();
        var apiKey = appSettings.GetValue < string > (APIKEY);
        if (!apiKey.Equals(extractedApiKey)) {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }
        await _next(context);
    }
}