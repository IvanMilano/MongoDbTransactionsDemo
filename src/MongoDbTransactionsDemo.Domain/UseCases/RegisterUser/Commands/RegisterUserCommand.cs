using CQRSlite.Commands;

using System;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Commands
{
    public class RegisterUserCommand : ICommand
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public RegisterUserCommand(Guid id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}