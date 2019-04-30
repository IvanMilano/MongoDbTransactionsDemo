using CQRSlite.Commands;
using System;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Commands
{
    public class ActivateUserCommand : ICommand
    {
        public Guid Id { get; }

        public ActivateUserCommand(Guid userId)
        {
            Id = userId;
        }
    }
}