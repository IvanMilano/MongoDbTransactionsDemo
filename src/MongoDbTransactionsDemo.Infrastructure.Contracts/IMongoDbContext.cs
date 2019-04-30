using MongoDB.Driver;

using System.Threading;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Infrastructure.Contracts
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }

        IClientSessionHandle Session { get; }

        Task<IClientSessionHandle> StartSession(CancellationToken cancellactionToken = default);
    }
}