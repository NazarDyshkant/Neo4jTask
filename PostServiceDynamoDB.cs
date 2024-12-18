using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading.Tasks;

namespace snmDB
{
    internal class PostServiceDynamoDB
    {
        private static readonly string tableName = "SocialNetworkPosts";

        public static async Task UpdateItemAsync(string pk, string sk, string newContent, string modifiedDateTime)
        {
            var client = new AmazonDynamoDBClient(); 

            var table = Table.LoadTable(client, tableName);

            var item = new Document
            {
                ["PK"] = pk,                   
                ["SK"] = sk,                      
                ["Content"] = newContent,       
                ["ModifiedDateTime"] = modifiedDateTime 
            };

            try
            {
                await table.PutItemAsync(item);
                Console.WriteLine("Item updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating item: " + ex.Message);
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter Post ID:");
            var postId = Console.ReadLine();

            Console.WriteLine("Enter Comment ID:");
            var commentId = Console.ReadLine();

            Console.WriteLine("Enter Updated Content:");
            var newContent = Console.ReadLine();

            Console.WriteLine("Enter Modified DateTime (e.g., 2024-06-12T12:00:00):");
            var modifiedDateTime = Console.ReadLine();

            var pk = $"POST#{postId}";
            var sk = $"COMMENT#{commentId}";

            await UpdateItemAsync(pk, sk, newContent, modifiedDateTime);
        }
    }
}
