using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Users.Queries
{
    public class AuthUserQuery : IRequest<BaseResult<bool>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthUserQueryHandler : IRequestHandler<AuthUserQuery, BaseResult<bool>>
    {
        private readonly IIdentityService _identityService;

        public AuthUserQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<BaseResult<bool>> Handle(AuthUserQuery request, CancellationToken cancellationToken)
        {
            var existsUser = await _identityService.AuthUser(request.Email, request.Password);

            if (existsUser)
            {
                return BaseResult<bool>.Success(existsUser, "");

            }
            return BaseResult<bool>.Failure(existsUser, "Invalid email or password");
        }
    }
}
