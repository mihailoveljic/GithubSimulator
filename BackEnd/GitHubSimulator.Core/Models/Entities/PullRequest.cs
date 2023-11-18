using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Entities;

sealed class PullRequest : Abstractions.Task
{
    public Guid Source { get; }
    public Guid Target { get; }
    public Guid? IssueId { get; }
    public Guid? MilestoneId { get; }

    private PullRequest(
        Guid source,
        Guid target,
        Guid? issueId,
        Guid? milestoneId,
        Guid id,
        IEnumerable<Event>? events) : base(id, TaskType.PullRequest, events)
    {
        Source = source;
        Target = target;
        IssueId = issueId;
        MilestoneId = milestoneId;
    }

    public static PullRequest Create(
        Guid source,
        Guid target,
        IEnumerable<Event>? events,
        Guid? issueId = null,
        Guid? milestoneId = null)
    {
        var validator = new PullRequestValidator();
        var pr = new PullRequest(
            source, 
            target, 
            issueId,
            milestoneId,
            Guid.NewGuid(),
            events);
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
            RuleFor(x => x.Source).NotNull().WithMessage("Source branch reference must not be null!")
                                  .NotEmpty().WithMessage("Source branch reference must not be empty!");
            RuleFor(x => x.Target).NotNull().WithMessage("Target branch reference must not be null!")
                                  .NotEmpty().WithMessage("Target branch reference must not be empty!");
        }
    }
}
