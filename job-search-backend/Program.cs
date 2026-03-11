using JobSearchAPI.job_search_backend.Services;
using JobSearchAPI.Services;
using Microsoft.AspNetCore.RateLimiting;

var useApiKey = false; //Set to true if you want to use API Key encryption 
var builder = WebApplication.CreateBuilder(args);
var frontendUrl = builder.Configuration["FRONTEND_URL"] ?? "http://localhost:5173";

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<RemotiveJobServices>();
builder.Services.AddHttpClient<AdzunaJobService>();

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobProvider, RemotiveJobServices>();
builder.Services.AddScoped<IJobProvider, AdzunaJobService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(frontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5;
        opt.QueueLimit = 1;
    });
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = 429; 
        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Try again in 20 seconds.", cancellationToken);
    };
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 10_000;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();