using CQRSlite.Events;

using MongoDB.Driver;

using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Events;
using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.ReadModel;
using MongoDbTransactionsDemo.Infrastructure.Contracts;

using System.Threading;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.EventHandlers
{
    public class UserReadModelEventHandler : IEventHandler<UserRegisteredEvent>, ICancellableEventHandler<UserActivatedAccountEvent>
    {
        private readonly IMongoDbContext _mongoDbContext;

        public UserReadModelEventHandler(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task Handle(UserRegisteredEvent message)
        {
            var userReadModel = new UserReadModel
            {
                Id = message.Id,
                FirstName = message.FirstName,
                LastName = message.LastName,
                Email = message.Email,
                IsAccountActivated = false
            };

            var session = _mongoDbContext.Session;
            var readModel = _mongoDbContext.Database.GetCollection<UserReadModel>("userreadmodel");
            await readModel.InsertOneAsync(session, userReadModel);
        }

        public async Task Handle(UserActivatedAccountEvent message, CancellationToken token = default)
        {
            var filter = Builders<UserReadModel>.Filter.Eq(x => x.Id, message.Id);
            var update = Builders<UserReadModel>.Update.Set(x => x.IsAccountActivated, true);
            var readModel = _mongoDbContext.Database.GetCollection<UserReadModel>("userreadmodel");
            await readModel.UpdateOneAsync(filter, update);            
        }
    }
}