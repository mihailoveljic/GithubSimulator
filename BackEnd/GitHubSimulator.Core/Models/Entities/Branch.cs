using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.Entities;

sealed class Branch : Entity
{
    public string Name { get; init; }
    public Guid RepositoryId { get; init; }
    public Guid? IssueId { get; init; }
    public IEnumerable<Commit>? Commits { get; init; }

    private Branch(
        Guid id,
        string name,
        Guid repositoryId,
        Guid? issueId,
        IEnumerable<Commit>? commits) : base(id)
    {
        Name = name;
        RepositoryId = repositoryId;
        IssueId = issueId;
        Commits = commits;
    }

    public static Branch Create(
        string name,
        Guid repositoryId,
        Guid? issueId = null,
        IEnumerable<Commit>? commits = null)
    {
        var validator = new BranchValidator();

        var branch = new Branch(
            Guid.NewGuid(),
            name,
            repositoryId,
            issueId,
            commits);

        var validatorResult = validator.Validate(branch);

        if (validatorResult.IsValid)
            return branch;

        throw new ValidationException(validatorResult.Errors);
    }

    private class BranchValidator : AbstractValidator<Branch>
    {
        public BranchValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Branch name must not be null!")
                                .NotEmpty().WithMessage("Branch name must not be empty!");
            RuleFor(x => x.RepositoryId).NotNull().WithMessage("Repository id must not be null!");
        }
    }
}
