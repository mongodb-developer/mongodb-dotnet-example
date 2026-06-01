using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using mongodb_dotnet_example.Controllers;
using mongodb_dotnet_example.Models;
using mongodb_dotnet_example.Services;
using Xunit;

namespace mongodb_dotnet_example.Tests
{
    public class GamesControllerTests
    {
        [Fact]
        public void Get_ReturnsAllGames()
        {
            var expectedGames = new List<Game>
            {
                new Game { Id = "0123456789abcdef01234567", Name = "Celeste", Price = 19.99m, Category = "Platformer" },
                new Game { Id = "fedcba987654321001234567", Name = "Hades", Price = 24.99m, Category = "Roguelike" }
            };
            var controller = new GamesController(new FakeGamesService(expectedGames));

            var result = controller.Get();

            Assert.Equal(expectedGames, result.Value);
        }

        [Fact]
        public void Get_WhenGameExists_ReturnsGame()
        {
            var game = new Game { Id = "0123456789abcdef01234567", Name = "Celeste", Price = 19.99m, Category = "Platformer" };
            var controller = new GamesController(new FakeGamesService(new List<Game> { game }));

            var result = controller.Get(game.Id);

            Assert.Equal(game, result.Value);
        }

        [Fact]
        public void Get_WhenGameIsMissing_ReturnsNotFound()
        {
            var controller = new GamesController(new FakeGamesService());

            var result = controller.Get("0123456789abcdef01234567");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult()
        {
            var controller = new GamesController(new FakeGamesService());
            var newGame = new Game { Name = "Stardew Valley", Price = 14.99m, Category = "Simulation" };

            var result = controller.Create(newGame);

            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal("GetGame", createdAtRouteResult.RouteName);
            Assert.Equal(newGame, createdAtRouteResult.Value);
            Assert.Equal(newGame.Id, createdAtRouteResult.RouteValues["id"]);
        }

        [Fact]
        public void Update_WhenGameExists_ReturnsNoContent()
        {
            var existingGame = new Game { Id = "0123456789abcdef01234567", Name = "Celeste", Price = 19.99m, Category = "Platformer" };
            var controller = new GamesController(new FakeGamesService(new List<Game> { existingGame }));
            var updatedGame = new Game { Id = existingGame.Id, Name = "Celeste", Price = 17.99m, Category = "Platformer" };

            var result = controller.Update(existingGame.Id, updatedGame);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_WhenGameExists_ReturnsNoContent()
        {
            var existingGame = new Game { Id = "0123456789abcdef01234567", Name = "Celeste", Price = 19.99m, Category = "Platformer" };
            var fakeService = new FakeGamesService(new List<Game> { existingGame });
            var controller = new GamesController(fakeService);

            var result = controller.Delete(existingGame.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(fakeService.Get(existingGame.Id));
        }

        private sealed class FakeGamesService : IGamesService
        {
            private readonly List<Game> games;

            public FakeGamesService(List<Game> seedGames = null)
            {
                games = seedGames ?? new List<Game>();
            }

            public List<Game> Get() => new List<Game>(games);

            public Game Get(string id) => games.Find(game => game.Id == id);

            public Game Create(Game game)
            {
                game.Id ??= "0123456789abcdef01234567";
                games.Add(game);
                return game;
            }

            public void Update(string id, Game updatedGame)
            {
                var index = games.FindIndex(game => game.Id == id);
                if (index >= 0)
                {
                    games[index] = updatedGame;
                }
            }

            public void Delete(Game gameForDeletion) => games.RemoveAll(game => game.Id == gameForDeletion.Id);

            public void Delete(string id) => games.RemoveAll(game => game.Id == id);

            public void SeedIfEmpty(IEnumerable<Game> seedGames)
            {
                if (games.Count == 0)
                {
                    games.AddRange(seedGames);
                }
            }
        }
    }
}
