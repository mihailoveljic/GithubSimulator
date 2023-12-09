using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.AggregateRoots
{
    public sealed class User : Entity
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Mail Mail { get; private set; }
        public AccountCredentials AccountCredentials { get; set; }

        private User(Guid id, string name, string surname, Mail mail, AccountCredentials accountCredentials) : base(id)
        {
            Name = name;
            Surname = surname;
            Mail = mail;
            AccountCredentials = accountCredentials;
        }

        public static User Create(string name, string surname, Mail mail, AccountCredentials accountCredentials, Guid? id = null)
        {
            var validator = new UserValidator();
            var user = new User(id is not null ? id.Value : Guid.NewGuid(), name, surname, mail, accountCredentials);

            var validationResult = validator.Validate(user);
            if (validationResult.IsValid)
            {
                return user;
            }
            throw new ValidationException(validationResult.Errors);
        }

        private class UserValidator : AbstractValidator<User>
        {
            public UserValidator()
            {
                RuleFor(x => x.Name).NotNull().WithMessage("Name must not be null!")
                                    .NotEmpty().WithMessage("Name must not be empty!");
                RuleFor(x => x.Surname).NotNull().WithMessage("Surname must not be null!")
                                     .NotEmpty().WithMessage("Surname must not be empty!");
            }
        }
    }
}
