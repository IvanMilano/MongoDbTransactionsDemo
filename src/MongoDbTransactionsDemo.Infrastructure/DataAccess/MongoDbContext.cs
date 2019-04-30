using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Events;
using MongoDbTransactionsDemo.Infrastructure.Contracts;

using System.Threading;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Infrastructure.DataAccess
{
    public class MongoDbContext : IMongoDbContext
    {
        public IMongoDatabase Database { get; }

        public IClientSessionHandle Session { get; private set; }

        private MongoClient _client;

        public MongoDbContext()
        {
            var settings = new MongoClientSettings
            {
                Servers = new[] 
                {
                    new MongoServerAddress("localhost", 37017),
                    new MongoServerAddress("localhost", 37018),
                    new MongoServerAddress("localhost", 37019)
                },
                ConnectionMode = ConnectionMode.ReplicaSet,
                ReadPreference = ReadPreference.Primary
            };

            _client = new MongoClient(settings);
            Database = _client.GetDatabase("usersystem");
            ClassMapping();
        }

        private static void ClassMapping()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserRegisteredEvent))) { BsonClassMap.RegisterClassMap<UserRegisteredEvent>(); }
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserActivatedAccountEvent))) { BsonClassMap.RegisterClassMap<UserActivatedAccountEvent>(); }
        }

        public async Task<IClientSessionHandle> StartSession(CancellationToken cancellactionToken = default)
        {
            var session = await _client.StartSessionAsync(cancellationToken: cancellactionToken);
            Session = session;
            return session;
        }
    }
}