using System;
using System.Windows;
using snmDB.Services;
using Neo4j.Driver;

namespace snmDB
{
    public partial class App : Application
    {
        public static MongoDbService MongoDbService { get; private set; }
        public static IAsyncSession Neo4jSession { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                MongoDbService = new MongoDbService("mongodb://localhost:27017", "SocialNetworkDB");
                var neo4jDriver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"));
                Neo4jSession = neo4jDriver.AsyncSession();

                Console.WriteLine("Services initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialization error: {ex.Message}");
            }
        }


        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Neo4jSession.CloseAsync().Wait();
        }
    }
}
