using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Refresh;
using EcomSystem.Contracts.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IRefreshTokenStore _store;
    private readonly IAuthUserRepository _authRepo;
    private readonly IJwtTokenGenerator _jwt;

    public RefreshTokenCommandHandler(
        IRefreshTokenStore store,
        IAuthUserRepository authRepo,
        IJwtTokenGenerator jwt)
    {
        _store = store;
        _authRepo = authRepo;
        _jwt = jwt;
    }

    public async Task<RefreshTokenResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        // 1. check refresh token
        var email = await _store.GetAsync(request.RefreshToken);

        if (email == null)
            throw new Exception("Invalid refresh token");

        // 2. lấy user từ AUTH DB (NOT UserService)
        var user = await _authRepo.GetByEmailAsync(email);

        if (user == null)
            throw new Exception("User not found");

      

        if (user.Status != "Active")
            throw new Exception("User inactive");

        // 3. generate new tokens
        var tokenUser = new TokenUserDto
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
        var accessToken = _jwt.GenerateAccessToken(tokenUser);
        var refreshToken = _jwt.GenerateRefreshToken();

        // 4. rotate token
        await _store.DeleteAsync(request.RefreshToken);
        await _store.SaveAsync(refreshToken, user.Email);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}