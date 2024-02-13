using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Entities;

public sealed class Milestone : Entity
{
    public string Title { get; init; }
    public string? Description { get; init; }
    public DateTime? DueDate { get; init; }
    public State State { get; init; }
    public Guid RepositoryId { get; init; }

    private Milestone(
        Guid id,
        string title, 
        string? description, 
        DateTime? dueDate, 
        State state,
        Guid repositoryId) : base(id)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        State = state;
        RepositoryId = repositoryId;
    }

    public static Milestone Create(
        string title,
        string? description,
        DateTime? dueDate,
        State state,
        Guid repositoryId,
        Guid? id = null)
    {
        var validator = new MilestoneValidator();

        var milestone = new Milestone(
            id is not null ? id.Value : Guid.NewGuid(),
            title,
            description,
            dueDate,
            state,
            repositoryId);

        var validatorResult = validator.Validate(milestone);

        if (validatorResult.IsValid)
            return milestone;

        throw new ValidationException(validatorResult.Errors);
    }

    private class MilestoneValidator : AbstractValidator<Milestone>
    {
        public MilestoneValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("Title must not be null!")
                                 .NotEmpty().WithMessage("Title must not be empty!");
            //RuleFor(x => x.Description).NotNull().WithMessage("Description must not be null!")
                                       //.NotEmpty().WithMessage("Description must not be empty!");
            //RuleFor(x => x.DueDate).NotNull().WithMessage("Due date must be defined!");
            RuleFor(x => x.State).Must(x => !x.Equals(State.Merged))
                                 .WithMessage("State can be Open or Closed, Merged is not valid");
        }
    }
}
