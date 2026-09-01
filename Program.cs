using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using mongodb_dotnet_example.Models;
using mongodb_dotnet_example.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GamesDatabaseSettings>(builder.Configuration.GetSection(nameof(GamesDatabaseSettings)));
builder.Services.Configure<StartupBehaviorSettings>(builder.Configuration.GetSection(nameof(StartupBehaviorSettings)));

builder.Services.AddSingleton<IGamesDatabaseSettings>(sp => sp.GetRequiredService<IOptions<GamesDatabaseSettings>>().Value);

builder.Services.AddSingleton<IGamesService, GamesService>();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "mongodb_dotnet_example", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "mongodb_dotnet_example v1"));

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseAuthorization();

var startupBehaviorOptions = app.Services.GetRequiredService<IOptions<StartupBehaviorSettings>>();
if (startupBehaviorOptions.Value.SeedOnStartup)
{
    var gamesService = app.Services.GetRequiredService<IGamesService>();
    gamesService.SeedIfEmpty(GameSeedData.DefaultGames);
}

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
