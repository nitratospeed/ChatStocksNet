using Application.Core.Users.Commands;
using Application.Core.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebUI.Controllers.v1
{
    public class UsersController : BaseApiController
    {
        [HttpGet("auth")]
        public async Task<IActionResult> Get(string email, string password)
        {
            return Ok(await Mediator.Send(new AuthUserQuery { Email = email, Password = password }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
