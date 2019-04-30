using CQRSlite.Commands;
using CQRSlite.Queries;

using Microsoft.AspNetCore.Mvc;

using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.Commands;
using MongoDbTransactionsDemo.Domain.UseCases.RegisterUser.ReadModel.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDbTransactionsDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryProcessor _queryProcessor;

        public UserController(ICommandSender commandSender, IQueryProcessor queryProcessor)
        {
            _commandSender = commandSender;
            _queryProcessor = queryProcessor;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequestDto registerUserDto, CancellationToken cancellationToken)
        {
            var userId = Guid.NewGuid();
            var createUserCommand = new RegisterUserCommand(userId, registerUserDto.FirstName, registerUserDto.LastName, registerUserDto.Email);
            await _commandSender.Send(createUserCommand, cancellationToken);
            return Created(string.Empty, userId);
        }

        [HttpPut]
        [Route("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Please provide userid");
            }

            var userModel = await _queryProcessor.Query(new GetUser(id));

            if(userModel == null)
            {
                return NotFound();
            }

            var activateUserCommand = new ActivateUserCommand(id);
            await _commandSender.Send(activateUserCommand);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest("Please provide userid");
            }

            var userModel = await _queryProcessor.Query(new GetUser(id));
            return Ok(userModel);
        }
    }
}