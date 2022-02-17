using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Core.Users.Commands
{
    public class CreateUserCommand : IRequest<BaseResult<int>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResult<int>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResult<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = new User
            {
                Email = request.Email,
                Password = request.Password,
                Fullname = request.FullName,
            };

            var result = await _userRepository.Insert(entity);

            return BaseResult<int>.Success(result.Id, "");
        }
    }
}
