using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EcomSystem.Contracts.Users;
using AuthService.Application.Interfaces;
using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.OAuth
{
    public class GoogleLoginHandler
    : IRequestHandler<GoogleLoginCommand, GoogleLoginResponse>
    {
        private readonly IGoogleAuthService _googleAuth;
        private readonly IUserServiceClient _userService;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IRefreshTokenStore _refreshStore;

        public GoogleLoginHandler(
            IGoogleAuthService googleAuth,
            IUserServiceClient userService,
            IJwtTokenGenerator jwt,
            IRefreshTokenStore refreshStore)
        {
            _googleAuth = googleAuth;
            _userService = userService;
            _jwt = jwt;
            _refreshStore = refreshStore;
        }

        public async Task<GoogleLoginResponse> Handle(
            GoogleLoginCommand request,
            CancellationToken cancellationToken)
        {
            // 1. verify google token
            var googleUser = await _googleAuth.ValidateAsync(request.IdToken);

            if (googleUser == null)
                throw new Exception("Invalid Google token");

            // 2. check user in USER SERVICE
            var user = await _userService.GetUserByEmailAsync(googleUser.Email);

            Guid userId;

            // 3. create user if not exists
            if (user == null)
            {
                userId = await _userService.CreateUserAsync(new CreateUserRequest
                {
                    Email = googleUser.Email,
                    FullName = googleUser.Name ?? "Google User"
                });
            }
            else
            {
                userId = user.Id;
            }

            // 4. create token user (internal auth model)
            var tokenUser = new TokenUserDto
            {
                UserId = userId,
                Email = googleUser.Email,
                Role = "User"
            };

            // 5. generate tokens
            var accessToken = _jwt.GenerateAccessToken(tokenUser);
            var refreshToken = _jwt.GenerateRefreshToken();

            await _refreshStore.SaveAsync(refreshToken, googleUser.Email);

            return new GoogleLoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}