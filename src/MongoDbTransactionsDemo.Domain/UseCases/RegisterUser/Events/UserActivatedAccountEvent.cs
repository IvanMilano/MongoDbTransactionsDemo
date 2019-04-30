using CQRSlite.Events;
using System;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Events
{
    public class UserActivatedAccountEvent : IEvent
    {
        public bool AccountIsActive { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}