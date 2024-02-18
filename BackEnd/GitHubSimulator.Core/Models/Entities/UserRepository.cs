using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Entities;

public class UserRepository : Entity
{
    public string UserName { get; init; }
    public string RepositoryName { get; init; }
    public UserRepositoryRole UserRepositoryRole { get; init; }

    private UserRepository(
            Guid id,
            string userName,
            string repoName,
            UserRepositoryRole role
        ) : base(id)
    {
        UserName = userName;
        RepositoryName = repoName;
        UserRepositoryRole = role;
    }

    public static UserRepository Create(
        string userName,
        string repoName,
        UserRepositoryRole role,
        Guid? id = null
    )
    {
        var validator = new UserRepositoryValidator();
        var userRepository = new UserRepository(
            id ?? Guid.NewGuid(),
            userName,
            repoName,
            role);
        var validationResult = validator.Validate(userRepository);
        if (validationResult.IsValid)
        {
            return userRepository;
        }

        throw new ValidationException(validationResult.Errors);
    }

    private class UserRepositoryValidator : AbstractValidator<UserRepository>
    {
        public UserRepositoryValidator()
        {
            RuleFor(x => x.RepositoryName).NotNull().WithMessage("Repository name must not be null!")
                .NotEmpty().WithMessage("Repository name must not be empty!");
            RuleFor(x => x.UserName).NotNull().WithMessage("Username must not be null!")
                .NotEmpty().WithMessage("Username must not be empty!");
            RuleFor(x => x.UserRepositoryRole).NotNull().WithMessage("Role must not be null!");
        }
    }
}