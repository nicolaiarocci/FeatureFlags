using FeatureFlags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"))
    .AddFeatureFilter<MyCustomFilter>();

var app = builder.Build();

var weatherforecastGroup = app.MapGroup("/weatherforecast")
    .AddEndpointFilter(new EndpointFilter("WeatherForecast"));

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async (IFeatureManager manager, [FromHeader(Name = "X-Lucky-Number")] int? inputNumber) =>
{

    if (!await manager.IsEnabledAsync("WeatherForecast", new MyCustomFilterContext { InputNumber = inputNumber ?? 0 }))
        return Results.NotFound();

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
