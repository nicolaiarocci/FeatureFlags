using FeatureFlags;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Register the Targeting context accessor
builder.Services.AddSingleton<ITargetingContextAccessor, HttpTargetingContextAccessor>();

builder.Services
    .AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"))
    .AddFeatureFilter<TargetingFilter>();

var app = builder.Build();

var weatherforecastGroup = app.MapGroup("/weatherforecast")
    .AddEndpointFilter(new FeatureFilter("WeatherForecast"));

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

weatherforecastGroup.MapGet("", () =>
{

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return Results.Ok(forecast);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
