using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Entities;

sealed class Milestone : Entity
{
    public string Title { get; }
    public string Description { get; }
    public DateTime DueDate { get; }
    public State State { get; }

    private Milestone(
        Guid id,
        string title, 
        string description, 
        DateTime dueDate, 
        State state) : base(id)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        State = state;
    }

    public static Milestone Create(
        string title,
        string description,
        DateTime dueDate,
        State state)
    {
        var validator = new MilestoneValidator();

        var milestone = new Milestone(
            Guid.NewGuid(),
            title,
            description,
            dueDate,
            state);

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
            RuleFor(x => x.Description).NotNull().WithMessage("Description must not be null!")
                                       .NotEmpty().WithMessage("Description must not be empty!");
            RuleFor(x => x.DueDate).NotNull().WithMessage("Due date must be defined!");
            RuleFor(x => x.State).Must(x => !x.Equals(State.Merged))
                                 .WithMessage("State can be Open or Closed, Merged is not valid");
        }
    }
}
