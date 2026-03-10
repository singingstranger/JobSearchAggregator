using JobSearchAPI.job_search_backend.Services;
using JobSearchAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var frontendUrl = builder.Configuration["FRONTEND_URL"] ?? "http://localhost:5173";

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

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

app.Run();