using CQRSlite.Domain;

using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Events;

using System;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.WriteModel
{
    public class User : AggregateRoot
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private bool _userHasActivatedAccount;

        public User() { }

        public User(Guid id, string firstName, string lastName, string email)
        {
            Id = id;         
            ApplyChange(new UserRegisteredEvent { Id = id, FirstName = firstName, LastName = lastName, Email = email });
        }

        private void Apply(UserRegisteredEvent @event)
        {
            _firstName = @event.FirstName;
            _lastName = @event.LastName;
            _email = @event.Email;
            _userHasActivatedAccount = false;
        }

        public Task ActivateAccount()
        {
            ApplyChange(new UserActivatedAccountEvent { AccountIsActive = true });
            return Task.CompletedTask;
        }

        private void Apply(UserActivatedAccountEvent @event)
        {
            _userHasActivatedAccount = true;
        }
    }
}