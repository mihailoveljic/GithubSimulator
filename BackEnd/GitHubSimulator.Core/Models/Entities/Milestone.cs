using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;

namespace GitHubSimulator.Core.Models.Entities;

public class Milestone : Entity
{
    [Key]
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime DueDate { get; }
    public State State { get; }
    public Guid RepositoryId { get; }

    private Milestone(
        Guid id,
        string title, 
        string description, 
        DateTime dueDate, 
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
        string description,
        DateTime dueDate,
        State state,
        Guid repositoryId)
    {
        var validator = new MilestoneValidator();

        var milestone = new Milestone(
            Guid.NewGuid(),
            title,
            description,
            dueDate,
            state,
            repositoryId);

        var validatorResult = validator.Validate(milestone);

        if (validatorResult.IsValid)
            return milestone;

        throw new FluentValidation.ValidationException(validatorResult.Errors);
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
