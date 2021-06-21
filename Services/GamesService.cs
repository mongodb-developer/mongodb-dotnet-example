using mongodb_dotnet_example.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace mongodb_dotnet_example.Services
{
    public class GamesService
    {
        private readonly IMongoCollection<Game> _games;

        public GamesService(IGamesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _games = database.GetCollection<Game>(settings.GamesCollectionName);
        }

        public List<Game> Get() => _games.Find(game => true).ToList();

        public Game Get(string id) => _games.Find(game => game.Id == id).FirstOrDefault();

        public Game Create(Game game)
        {
            _games.InsertOne(game);
            return game;
        }

        public void Update(string id, Game updatedGame) => _games.ReplaceOne(game => game.Id == id, updatedGame);

        public void Delete(Game gameForDeletion) => _games.DeleteOne(game => game.Id == gameForDeletion.Id);

        public void Delete(string id) => _games.DeleteOne(game => game.Id == id);
    }
}