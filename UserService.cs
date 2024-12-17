using MongoDB.Driver;
using Neo4j.Driver;
using snmDB.Models;

namespace snmDB.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly Neo4jService _neo4jService;

        public UserService(MongoDbService mongoDbService)
        {

            _users = mongoDbService.GetUserCollection();
        }

        public UserService(MongoDbService mongoDbService, Neo4jService neo4jService)
        {
            _users = mongoDbService.GetUserCollection();
            _neo4jService = neo4jService;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            // Add user to MongoDB
            await _users.InsertOneAsync(user);

            // Create a node in Neo4j
            await _neo4jService.CreateUserNodeAsync(user.Email, user.FirstName, user.LastName);
        }

        public async Task DeleteUserAsync(string email)
        {
            // Remove user from MongoDB
            await _users.DeleteOneAsync(user => user.Email == email);

            // Remove user node from Neo4j
            await _neo4jService.DeleteUserNodeAsync(email);
        }

        public async Task CreateRelationshipAsync(string userEmail1, string userEmail2, string relationshipType)
        {
            // Create a relationship in Neo4j
            await _neo4jService.CreateRelationshipAsync(userEmail1, userEmail2, relationshipType);
        }

        public async Task DeleteRelationshipAsync(string userEmail1, string userEmail2, string relationshipType)
        {
            // Delete a relationship in Neo4j
            await _neo4jService.DeleteRelationshipAsync(userEmail1, userEmail2, relationshipType);
        }

        public async Task<bool> CheckRelationshipAsync(string userEmail1, string userEmail2)
        {
            // Check for a relationship in Neo4j
            return await _neo4jService.CheckConnectionAsync(userEmail1, userEmail2);
        }

        public async Task<int?> GetDistanceBetweenUsersAsync(string userEmail1, string userEmail2)
        {
            // Get the shortest path distance between users in Neo4j
            return await _neo4jService.GetDistanceAsync(userEmail1, userEmail2);
        }
    }
}
