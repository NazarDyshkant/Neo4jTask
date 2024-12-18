using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using System.Windows.Media.Animation;

namespace snmDB
{
    internal class DynamoDBsetup
    {
        private static readonly string tableName = "SocialNetworkPosts";

        public static async Task CreateTableAsync()
        {
            var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);

            var request = new CreateTableRequest
            {
                TableName = tableName,
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement("PK", KeyType.HASH), // Partition Key
                new KeySchemaElement("SK", KeyType.RANGE) // Sort Key
            },
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition("PK", ScalarAttributeType.S),
                new AttributeDefinition("SK", ScalarAttributeType.S)
            },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            try
            {
                var response = await client.CreateTableAsync(request);
                Console.WriteLine("Table created successfully: " + response.TableDescription.TableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating table: " + ex.Message);
            }
        }

        static async Task Main(string[] args)
        {
            await CreateTableAsync();
        }
    }
}
