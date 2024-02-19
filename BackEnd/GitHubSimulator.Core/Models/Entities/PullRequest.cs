using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.Entities;

public sealed class PullRequest : Abstractions.Task
{
    public Guid? Source { get; init; }
    public Guid? Target { get; init; }
    public Guid? IssueId { get; init; }
    public Guid? MilestoneId { get; init; }
    public Guid? RepositoryId { get; init; }
    public string Assignee { get; set; }
    public string[]? Assignees { get; set; }
    public string Base { get; set; }
    public string Body { get; set; }
    public string Head { get; set; }
    public string Title { get; set; }
    public string RepoName { get; set; }
    public bool IsOpen { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Author { get; init; }
    public int? Number { get; set; }



    private PullRequest(
        Guid? source,
        Guid? target, 
        Guid? issueId,
        Guid? milestoneId,
        Guid? repositoryId,
        string assignee,
        string[]? assignees,
        string @base,
        string body,
        string head,
        string title,
        string repoName,
        bool isOpen,
        DateTime createdAt,
        string author,
        int? number,
        Guid id,
        IEnumerable<Event>? events,
        IEnumerable<Label>? labels) : base(id, TaskType.PullRequest, events, labels)
    {
        Source = source;
        Target = target;
        IssueId = issueId;
        MilestoneId = milestoneId;
        RepositoryId = repositoryId;
        Assignee = assignee;
        Assignees = assignees;
        Base = @base;
        Body = body;
        Head = head;
        Title = title;
        RepoName = repoName;
        IsOpen = isOpen;
        CreatedAt = createdAt;
        Author = author;
        Number = number;
    }

    public static PullRequest Create(
        Guid? source,
        Guid? target,
        string assignee,
        string @base,
        string body,
        string head,
        string title,
        string repoName,
        bool isOpen,
        string author,
        DateTime createdAt,
        string[]? assignees,
        int? number,
        Guid? issueId = null,
        Guid? milestoneId = null,
        Guid? repositoryId = null,
        IEnumerable<Event>? events = null,
        IEnumerable<Label>? labels =null,
        Guid? id = null
        )
    {
        var validator = new PullRequestValidator();
        var pr = new PullRequest(
            source,
            target,
            issueId,
            milestoneId,
            repositoryId,
            assignee,
            assignees,
            @base,
            body,
            head,
            title,
            repoName,
            isOpen,
            createdAt,
            author,
            number,
            id is null ? Guid.NewGuid() : id.Value,
            events,
            labels);

        var validationResult = validator.Validate(pr);
        if (validationResult.IsValid)
        {
            return pr;
        }
        throw new ValidationException(validationResult.Errors);
    }

    private class PullRequestValidator : AbstractValidator<PullRequest>
    {
        public PullRequestValidator()
        {
            RuleFor(x => x.Base).NotNull().WithMessage("Source branch reference must not be null!")
                                  .NotEmpty().WithMessage("Source branch reference must not be empty!");
            RuleFor(x => x.Head).NotNull().WithMessage("Target branch reference must not be null!")
                                  .NotEmpty().WithMessage("Target branch reference must not be empty!");
        }
    }
}
