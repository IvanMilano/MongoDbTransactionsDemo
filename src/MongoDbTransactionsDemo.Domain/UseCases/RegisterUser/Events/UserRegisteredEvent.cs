using CQRSlite.Events;

using System;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Events
{
    public class UserRegisteredEvent : IEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}