using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application.Interfaces;
using AuthService.Application.DTOs;

namespace AuthService.Application.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserServiceClient _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenStore _refreshTokenStore;

        private readonly IAuthUserRepository _authRepository;

        public LoginCommandHandler(
            IUserServiceClient userService,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenStore refreshTokenStore,
            IAuthUserRepository authRepository)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenStore = refreshTokenStore;
            _authRepository = authRepository;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Get user info (public data only)
            var user = await _userService.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                return new LoginResponse
                {
                    AccessToken = null,
                    RefreshToken = null
                };
            }

            // 2. LẤY AUTH DATA từ AUTH SERVICE (KHÔNG phải UserService)
            var authUser = await _authRepository.GetByEmailAsync(request.Email);

            if (authUser == null)
                throw new Exception("User not found in auth system");

            // 3. Verify password
            var isValid = _passwordHasher.Verify(request.Password, authUser.PasswordHash);
            var tokenUser = new TokenUserDto
            {
                UserId = authUser.Id,
                Email = authUser.Email,
                Role = authUser.Role
            };

            if (!isValid)
                throw new Exception("Invalid password");

            // 4. Generate tokens
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(tokenUser); // dùng public info
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            // 5. Save refresh token
            await _refreshTokenStore.SaveAsync(refreshToken, user.Email);

            // 6. Return response
            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
