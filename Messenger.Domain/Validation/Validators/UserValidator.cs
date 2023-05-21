using FluentValidation;
using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Models;

namespace Messenger.Domain.Validation.Validators;

public class UserValidator : AbstractValidator<User>
{
    private const string UsernameRegex = "^[a-zA-Z0-9_]+$";
    private const string NameRegex = "^[a-zA-ZА-яа-яЁё ]+$";
    
    public const int MinimumPasswordLength = 6;
    public const int MaximumPasswordLength = 32;

    public const int MinimumUsernameLength = 5;
    public const int MaximumUsernameLength = 50; 
    
    public const int MinimumNameLength = 1;
    public const int MaximumNameLength = 100;
    
    public UserValidator()
    {
        RuleFor(user => user.Email)
            .EmailAddress()
            .WithMessage(UserValidationErrorMessages.IncorrectEmail);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.EmptyEmail);

        RuleFor(user => user.Username)
            .MaximumLength(MaximumUsernameLength)
            .WithMessage(UserValidationErrorMessages.TooLongUsername);
        RuleFor(user => user.Username)
            .MinimumLength(MinimumUsernameLength)
            .WithMessage(UserValidationErrorMessages.TooShortUsername);
        RuleFor(user => user.Username)
            .Matches(UsernameRegex)
            .WithMessage(UserValidationErrorMessages.WrongUsername);

        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.EmptyName);
        RuleFor(user => user.Name)
            .MinimumLength(MinimumNameLength)
            .WithMessage(UserValidationErrorMessages.TooShortName);
        RuleFor(user => user.Name)
            .MaximumLength(MaximumNameLength)
            .WithMessage(UserValidationErrorMessages.TooLongName);
        RuleFor(user => user.Name)
            .Matches(NameRegex)
            .WithMessage(UserValidationErrorMessages.WrongName);

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.EmptyPassword);
        RuleFor(user => user.Password)
            .MinimumLength(MinimumPasswordLength)
            .WithMessage(UserValidationErrorMessages.TooShortPassword);
        RuleFor(user => user.Password)
            .MaximumLength(MaximumPasswordLength)
            .WithMessage(UserValidationErrorMessages.TooLongPassword);
    }
}