using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.Entities;

public class Issue : Abstractions.Task
{
    public string Title { get; }
    public string Description { get; }
    public DateTime CreatedAt { get; }
    public Mail Assigne { get; }
    public Guid RepositoryId { get; }
    public Guid? MilestoneId { get; }

    private Issue(
        Guid id,
        string title, 
        string description, 
        DateTime createdAt, 
        Mail assigne,
        Guid repositoryId,
        Guid? milestoneId,
        IEnumerable<Event>? events) : base(id, TaskType.Issue, events)
    {
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        Assigne = assigne;
        RepositoryId = repositoryId;
        MilestoneId = milestoneId;
    }

    public static Issue Create(
        string title,
        string description,
        Mail assigne,
        Guid repositoryId,
        Guid? milestoneId = null,
        IEnumerable<Event>? events = null)
    {
        var validator = new IssueValidator();

        var comment = new Issue(
            Guid.NewGuid(),
            title,
            description,
            DateTime.Now,
            assigne,
            repositoryId,
            milestoneId,
            events);

        var validatorResult = validator.Validate(comment);

        if (validatorResult.IsValid)
            return comment;

        throw new ValidationException(validatorResult.Errors);
    }

    private class IssueValidator : AbstractValidator<Issue>
    {
        public IssueValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("Title must not be null!")
                                   .NotEmpty().WithMessage("Title must not be empty!");
            RuleFor(x => x.Description).NotNull().WithMessage("Description must not be null!")
                                   .NotEmpty().WithMessage("Description must not be empty!");
            RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date Created must not be null!");
            RuleFor(x => x.RepositoryId).NotNull().WithMessage("Repository reference must not be null!");
        }
    }
}
