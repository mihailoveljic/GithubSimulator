using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.ValueObjects;

public class Mail : ValueObject
{
    public string Email { get; init; } = null!;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
    }

    private Mail(string email)
    {
        Email = email;
    }

    public static Mail Create(string emailAddress)
    {
        var validator = new MailValidator();
        var email = new Mail(emailAddress);
        var validationResult = validator.Validate(email);
        if (validationResult.IsValid)
        {
            return email;
        }
        throw new ValidationException(validationResult.Errors);
    }

    private class MailValidator : AbstractValidator<Mail>
    {
        public MailValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Email must not be null!")
                                 .NotEmpty().WithMessage("Email must not be empty!")
                                 .EmailAddress().WithMessage("Email is not valid!");
        }
    }
}
