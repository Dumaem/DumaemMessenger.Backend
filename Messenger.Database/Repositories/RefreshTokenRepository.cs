﻿using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

namespace Messenger.Database.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    public Task CreateTokenAsync(RefreshToken refreshToken)
    {
        return Task.CompletedTask;
    }

    public Task<RefreshToken?> GetTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task UseTokenAsync()
    {
        throw new NotImplementedException();
    }
}