using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.Entities;

public class Branch : Entity
{
    public string Name { get; }


    private Branch(
        Guid id,
        string name) : base(id)
    {
        Name = name;
    }

    public static Branch Create(
        string name)
    {
        var validator = new BranchValidator();

        var branch = new Branch(
            Guid.NewGuid(),
            name);

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
        }
    }
}
