using MongoDB.Driver;
using snmDB.Models;

namespace snmDB.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
                // Метод для отримання колекції постів
        public IMongoCollection<Post> GetPostCollection()
        {
            return _database.GetCollection<Post>("Posts");  // Повертає колекцію "Posts"
        }

        // Метод для отримання колекції користувачів
        public IMongoCollection<User> GetUserCollection()
        {
            return _database.GetCollection<User>("Users");  // Повертає колекцію "Users"
        }
    }
}
