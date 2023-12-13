using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.ValueObjects
{
    public sealed class AccountCredentials : ValueObject
    {
        public string UserName { get; init; }
        public string PasswordHash { get; init; }

        private AccountCredentials(string userName, string password)
        {
            UserName = userName;
            PasswordHash = password;
        }

        public static AccountCredentials Create(string userName, string password)
        {
            var validator = new AccountCredentialsValidator();

            var accountCredentials = new AccountCredentials(userName, password);

            var validationResult = validator.Validate(accountCredentials);
            if (validationResult.IsValid)
            {
                return accountCredentials;
            }

            throw new ValidationException(validationResult.Errors);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

        private class AccountCredentialsValidator : AbstractValidator<AccountCredentials>
        {
            public AccountCredentialsValidator()
            {
                RuleFor(x => x.UserName).NotNull().WithMessage("UserName must not be null!")
                                        .NotEmpty().WithMessage("UserName must not be empty!");
                RuleFor(x => x.PasswordHash).NotNull().WithMessage("Password must not be null!")
                                        .NotEmpty().WithMessage("Password must not be empty!");
            }
        }
    }
}
