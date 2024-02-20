using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.Entities;

public sealed class Issue : Abstractions.Task
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public Mail Assignee { get; init; }
    public Mail Author { get; init; }
    public Guid RepositoryId { get; init; }
    public Guid? MilestoneId { get; init; }

    public bool IsOpen { get; init; }

    private Issue(
        Guid id,
        string title, 
        string description, 
        DateTime createdAt, 
        Mail assignee,
        Mail author,
        Guid repositoryId,
        Guid? milestoneId,
        IEnumerable<Event>? events,
        IEnumerable<Label>? labels) : base(id, TaskType.Issue, events, labels)
    {
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        Assignee = assignee;
        Author = author;
        RepositoryId = repositoryId;
        MilestoneId = milestoneId;
        IsOpen = true;
    }

    public static Issue Create(
        string title,
        string description,
        Mail assigne,
        Mail author,
        Guid repositoryId,
        Guid? milestoneId = null,
        IEnumerable<Event>? events = null,
        IEnumerable<Label>? labels = null,
        Guid? id = null)
    {
        var validator = new IssueValidator();

        var issue = new Issue(
            id is not null ? id.Value : Guid.NewGuid(),
            title,
            description,
            DateTime.Now,
            assigne,
            author,
            repositoryId,
            milestoneId,
            events,
            labels);
        
        var validatorResult = validator.Validate(issue);

        if (validatorResult.IsValid)
            return issue;

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
