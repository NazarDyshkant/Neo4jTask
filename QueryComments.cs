using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

namespace snmDB
{
    internal class QueryComments
    {
        private static readonly string tableName = "SocialNetworkPosts";
        public static async Task QueryCommentsAsync(string postId)
        {

            var client = new AmazonDynamoDBClient();

            var table = Table.LoadTable(client, tableName);

            var filter = new QueryFilter("PK", QueryOperator.Equal, $"POST#{postId}");
            filter.AddCondition("SK", QueryOperator.BeginsWith, "COMMENT#");

            var search = table.Query(filter);

            try
            {
                var documents = await search.GetRemainingAsync();

                foreach (var doc in documents)
                {
                    Console.WriteLine($"Comment: {doc["Content"]}, ModifiedDateTime: {doc["ModifiedDateTime"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying comments: " + ex.Message);
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the Post ID to retrieve comments:");
            var postId = Console.ReadLine();

            await QueryCommentsAsync(postId);
        }
    }
}
