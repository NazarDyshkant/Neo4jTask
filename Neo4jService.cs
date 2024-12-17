using Neo4j.Driver;
using System;
using System.Threading.Tasks;

namespace snmDB.Services
{
    public class Neo4jService : IDisposable
    {
        private readonly IDriver _driver;

        public Neo4jService(string uri, string username, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
        }

        public async Task CreateUserNodeAsync(string userId, string firstName, string lastName)
        {
            var query = @"
                CREATE (u:User {userId: $userId, firstName: $firstName, lastName: $lastName})
                RETURN u";

            var parameters = new
            {
                userId,
                firstName,
                lastName
            };

            await ExecuteWriteAsync(query, parameters);
        }

        public async Task DeleteUserNodeAsync(string userId)
        {
            var query = "MATCH (u:User {userId: $userId}) DETACH DELETE u";
            var parameters = new { userId };
            await ExecuteWriteAsync(query, parameters);
        }

        public async Task CreateRelationshipAsync(string fromUserId, string toUserId, string relationshipType)
        {
            var query = $@"
                MATCH (a:User {{userId: $fromUserId}})
                MATCH (b:User {{userId: $toUserId}})
                MERGE (a)-[r:{relationshipType.ToUpper()}]->(b)
                RETURN r";

            var parameters = new
            {
                fromUserId,
                toUserId
            };

            await ExecuteWriteAsync(query, parameters);
        }

        public async Task DeleteRelationshipAsync(string fromUserId, string toUserId, string relationshipType)
        {
            var query = $@"
                MATCH (a:User {{userId: $fromUserId}})
                -[r:{relationshipType.ToUpper()}]->(b:User {{userId: $toUserId}})
                DELETE r";

            var parameters = new
            {
                fromUserId,
                toUserId
            };

            await ExecuteWriteAsync(query, parameters);
        }

        public async Task<bool> CheckConnectionAsync(string fromUserId, string toUserId)
        {
            var query = @"
                MATCH (a:User {userId: $fromUserId})-[:FRIEND*1..]->(b:User {userId: $toUserId})
                RETURN COUNT(*) > 0 AS isConnected";

            var parameters = new
            {
                fromUserId,
                toUserId
            };

            return await ExecuteReadAsync<bool>(query, parameters);
        }

        public async Task<int> GetDistanceAsync(string fromUserId, string toUserId)
        {
            var query = @"
                MATCH p = shortestPath((a:User {userId: $fromUserId})-[:FRIEND*]-(b:User {userId: $toUserId}))
                RETURN length(p) AS distance";

            var parameters = new
            {
                fromUserId,
                toUserId
            };

            return await ExecuteReadAsync<int>(query, parameters, -1); // Return -1 if no path exists
        }

        private async Task ExecuteWriteAsync(string query, object parameters)
        {
            await using var session = _driver.AsyncSession();
            try
            {
                await session.WriteTransactionAsync(async tx =>
                {
                    await tx.RunAsync(query, parameters);
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        private async Task<T> ExecuteReadAsync<T>(string query, object parameters, T defaultValue = default)
        {
            await using var session = _driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, parameters);
                    var record = await result.FetchAsync(); // Fetch the first record asynchronously
                    return record != null ? record.As<T>() : defaultValue; // Convert to T or return default value
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }



        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
