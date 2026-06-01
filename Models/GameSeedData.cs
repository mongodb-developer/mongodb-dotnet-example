using System.Collections.Generic;

namespace mongodb_dotnet_example.Models
{
    public static class GameSeedData
    {
        public static readonly IReadOnlyList<Game> DefaultGames = new List<Game>
        {
            new Game { Name = "Celeste", Price = 19.99m, Category = "Platformer" },
            new Game { Name = "Hades", Price = 24.99m, Category = "Roguelike" },
            new Game { Name = "Stardew Valley", Price = 14.99m, Category = "Simulation" },
            new Game { Name = "Forza Horizon 5", Price = 59.99m, Category = "Racing" },
            new Game { Name = "Minecraft", Price = 29.99m, Category = "Sandbox" }
        };
    }
}