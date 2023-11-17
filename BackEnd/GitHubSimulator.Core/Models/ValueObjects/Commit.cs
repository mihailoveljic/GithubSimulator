using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.ValueObjects;

sealed class Commit : ValueObject
{
    public DateTime OccuredAt { get; }
    public string Description { get; }
    public Guid Hash { get; }

    private Commit(
        DateTime occuredAt, 
        string description, 
        Guid hash)
    {
        OccuredAt = occuredAt;
        Description = description;
        Hash = hash;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return OccuredAt;
        yield return Description;
        yield return Hash;
    }

    public static Commit Create(
        string description,
        Guid hash)
    {
        var validator = new CommitValidator();

        var commit = new Commit(
            DateTime.Now,
            description,
            hash);

        var validatorResult = validator.Validate(commit);

        if (validatorResult.IsValid)
            return commit;

        throw new ValidationException(validatorResult.Errors);
    }

    private class CommitValidator : AbstractValidator<Commit>
    {
        public CommitValidator()
        {
            RuleFor(x => x.OccuredAt).NotNull().WithMessage("DateTime when Commit is created must not be null!");
            RuleFor(x => x.Description).NotNull().WithMessage("Description must not be null!")
                                       .NotEmpty().WithMessage("Description must not be empty!");
            RuleFor(x => x.Hash).NotNull().WithMessage("Commit Hash must not be null!");
        }
    }
}
