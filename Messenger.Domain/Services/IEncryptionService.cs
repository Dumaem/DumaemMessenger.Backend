﻿namespace Messenger.Domain.Services;

public interface IEncryptionService
{
    public Task<string> EncryptStringAsync(string content);
}