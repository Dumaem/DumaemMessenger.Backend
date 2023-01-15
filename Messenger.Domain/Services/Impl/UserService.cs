using FluentValidation;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

namespace Messenger.Domain.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IValidator<User> _userDataValidator;

    public UserService(IUserRepository userRepository, IEncryptionService encryptionService, IValidator<User> userDataValidator)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _userDataValidator = userDataValidator;
    }

    public async Task<int> CreateUserAsync(User user, string password)
    {
        await _userDataValidator.ValidateAndThrowAsync(user);
        
        var encryptedPassword = await _encryptionService.EncryptString(password);
        return await _userRepository.CreateUserAsync(user, encryptedPassword);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckUserPasswordAsync(int userId, string password)
    {
        var encryptedPassword = await _encryptionService.EncryptString(password);
        var storedUserPassword = await _userRepository.GetUserEncryptedPassword(userId);

        return encryptedPassword.Equals(storedUserPassword);
    }
}