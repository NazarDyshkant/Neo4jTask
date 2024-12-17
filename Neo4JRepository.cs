using Neo4jClient;

public class Neo4JRepository
{
    private readonly IGraphClient _client;

    public Neo4JRepository(string uri, string username, string password)
    {
        _client = new BoltGraphClient(new Uri(uri), username, password);
        _client.ConnectAsync().Wait();
    }

    // Створення вузла
    public async Task CreateUserNode(string userId, string name)
    {
        await _client.Cypher
            .Create("(u:User {UserId: $userId, Name: $name})")
            .WithParams(new { userId, name })
            .ExecuteWithoutResultsAsync();
    }

    // Видалення вузла
    public async Task DeleteUserNode(string userId)
    {
        await _client.Cypher
            .Match("(u:User {UserId: $userId})")
            .DetachDelete("u")
            .WithParam("userId", userId)
            .ExecuteWithoutResultsAsync();
    }

    // Створення зв'язку
    public async Task CreateFriendship(string userId1, string userId2)
    {
        await _client.Cypher
            .Match("(u1:User {UserId: $userId1}), (u2:User {UserId: $userId2})")
            .Create("(u1)-[:FRIEND]->(u2)")
            .WithParams(new { userId1, userId2 })
            .ExecuteWithoutResultsAsync();
    }

    // Видалення зв'язку
    public async Task DeleteFriendship(string userId1, string userId2)
    {
        await _client.Cypher
            .Match("(u1:User {UserId: $userId1})-[r:FRIEND]->(u2:User {UserId: $userId2})")
            .Delete("r")
            .WithParams(new { userId1, userId2 })
            .ExecuteWithoutResultsAsync();
    }
}
