using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.ReadModel
{
    public class UserReadModel
    {
        public const string IdFieldName = "_id";

        [BsonElement(IdFieldName)]
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("firstname")]
        public string FirstName { get; set; }
        [BsonElement("lastname")]
        public string LastName { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("isaccountactivated")]
        public bool IsAccountActivated { get; set; }
    }
}