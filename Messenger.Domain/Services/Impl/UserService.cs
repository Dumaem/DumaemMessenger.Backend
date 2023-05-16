using FluentValidation;
using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IValidator<User> _userDataValidator;

    public UserService(IUserRepository userRepository, IEncryptionService encryptionService,
        IValidator<User> userDataValidator)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _userDataValidator = userDataValidator;
    }

    public async Task<int> CreateUserAsync(User user, string password)
    {
        await _userDataValidator.ValidateAndThrowAsync(user);

        var encryptedPassword = await _encryptionService.EncryptStringAsync(password);
        return await _userRepository.CreateUserAsync(user, encryptedPassword);
    }

    public async Task<User?> GetUserAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public async Task<User?> GetUserAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<IEnumerable<User>?> GetUsersAsync()
    {
        return await _userRepository.GetUsers();
    }

    public async Task<IEnumerable<User>?> GetUsersAsync(int count, int offset)
    {
        return await _userRepository.GetUsers(count, offset);
    }

    public async Task<bool> CheckUserPasswordAsync(int userId, string password)
    {
        var encryptedPassword = await _encryptionService.EncryptStringAsync(password);
        var storedUserPassword = await _userRepository.GetUserEncryptedPassword(userId);

        return encryptedPassword.Equals(storedUserPassword);
    }

    public async Task<EntityResult<User>> ChangeName(int id, string name)
    {
        var res = await _userRepository.ChangeName(id, name);
        return res is not null
            ? new EntityResult<User> {Success = true, Entity = res}
            : new EntityResult<User> {Success = false, Message = UserErrorMessage.NotExistUser};
    }
    public async Task<EntityResult<User>> ChangeUsername(int id, string username)
    {
        var res = await _userRepository.ChangeUsername(id, username);
        return res is not null
            ? new EntityResult<User> {Success = true, Entity = res}
            : new EntityResult<User> {Success = false, Message = UserErrorMessage.NotExistUser};
    }

    public async Task<EntityResult<User>> ChangeEmail(int id, string email)
    {
        var res = await _userRepository.ChangeEmail(id, email);
        return res is not null
            ? new EntityResult<User> {Success = true, Entity = res}
            : new EntityResult<User> {Success = false, Message = UserErrorMessage.NotExistUser};
    }
}