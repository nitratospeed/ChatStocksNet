using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Users.Commands
{
    public class CreateUserCommand : IRequest<BaseResult<bool>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResult<bool>>
    {
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<BaseResult<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterUser(request.Email, request.Password);

            return BaseResult<bool>.Success(result, "");
        }
    }
}
