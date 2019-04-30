using CQRSlite.Commands;
using CQRSlite.Domain;

using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Commands;
using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.WriteModel;

using System.Threading;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.CommandHandlers
{
    public class UserCommandHandler : ICancellableCommandHandler<RegisterUserCommand>, ICancellableCommandHandler<ActivateUserCommand>
    {
        private readonly ISession _session;

        public UserCommandHandler(ISession session)
        {
            _session = session;
        }

        public async Task Handle(RegisterUserCommand command, CancellationToken token = default)
        {
            var user = new WriteModel.User(command.Id, command.FirstName, command.LastName, command.Email);
            await _session.Add(user);
            await _session.Commit(token);
        }

        public async Task Handle(ActivateUserCommand message, CancellationToken token = default)
        {
            var user = await _session.Get<User>(message.Id);
            await user.ActivateAccount();
            await _session.Commit(token);
        }
    }
}