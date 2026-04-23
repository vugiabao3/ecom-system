using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Constants;
using UserService.Domain.Entities;

namespace UserService.Application.Users.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // 1. tạo entity
            var user = new User
            {
                Id = request.Id, // 🔥 FIX QUAN TRỌNG NHẤt
                Email = request.Email,
                FullName = request.FullName,
              
            };

            // 2. lưu DB
            await _userRepository.AddAsync(user);
            await _userRepository.AddActivityLogAsync(new UserActivityLog
            {
                
                Action = ActivityActions.CreateUser,
                Description = "User was CreateUser"
            });
            await _userRepository.SaveChangesAsync(); // 🔥 BẮT BUỘC
            // 3. trả response
            return new CreateUserResponse
            {
                UserId = user.Id,
                Email = user.Email,
            };
        }
    }
}