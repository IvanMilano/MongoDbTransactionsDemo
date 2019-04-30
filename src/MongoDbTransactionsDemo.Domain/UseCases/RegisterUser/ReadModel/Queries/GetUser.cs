using CQRSlite.Queries;

using System;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.ReadModel.Queries
{
    public class GetUser : IQuery<UserReadModel>
    {
        public GetUser(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}