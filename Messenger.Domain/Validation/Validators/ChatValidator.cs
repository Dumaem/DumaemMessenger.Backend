using FluentValidation;
using Messenger.Domain.Models;

namespace Messenger.Domain.Validation.Validators;

public class ChatValidator : AbstractValidator<Chat>
{
    public const int MaximumNameLength = 50;
}