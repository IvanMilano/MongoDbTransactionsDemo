using CQRSlite.Queries;

using MongoDB.Driver;

using MongoDbTransactionsDemo.Infrastructure.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.ReadModel.Queries
{
    public class UserQueryHandler : ICancellableQueryHandler<GetUser, UserReadModel>
    {
        private readonly IMongoCollection<UserReadModel> _userReadModel;

        public UserQueryHandler(IMongoDbContext mongoDbContext)
        {
            _userReadModel = mongoDbContext.Database.GetCollection<UserReadModel>("userreadmodel");
        }

        public async Task<UserReadModel> Handle(GetUser message, CancellationToken token = default)
        {
            var filterBuilder = Builders<UserReadModel>.Filter;
            var filter = filterBuilder.Eq(UserReadModel.IdFieldName, message.Id);

            var result = await _userReadModel.FindAsync(filter, cancellationToken: token);
            return await result.SingleOrDefaultAsync(token);
        }
    }
}