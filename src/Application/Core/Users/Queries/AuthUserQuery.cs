using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Users.Queries
{
    public class AuthUserQuery : IRequest<BaseResult<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthUserQueryHandler : IRequestHandler<AuthUserQuery, BaseResult<string>>
    {
        private readonly IUserRepository _userRepository;

        public AuthUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResult<string>> Handle(AuthUserQuery request, CancellationToken cancellationToken)
        {
            var existsUser = await _userRepository.AnyFilter(x => x.Email == request.Email && x.Password == request.Password);

            if (existsUser)
            {
                var user = await _userRepository.GetByFilter(x => x.Email == request.Email);
                return BaseResult<string>.Success(user.Fullname, "");
                
            }
            return BaseResult<string>.Failure("", "Invalid user or password");
        }
    }
}
