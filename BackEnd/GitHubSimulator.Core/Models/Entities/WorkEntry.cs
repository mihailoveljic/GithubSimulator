using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.Entities;

sealed class WorkEntry : Entity
{
    public string Description { get; }
    public DateRange DateRange { get; }
    public Guid IssueId { get; }

    private WorkEntry(
        string description, 
        DateRange dateRange,
        Guid id,
        Guid issueId) : base(id)
    {
        Description = description;
        DateRange = dateRange;
        IssueId = issueId;
    }

    public static WorkEntry Create(
        string description,
        DateRange dateRange,
        Guid issueId)
    {
        var validator = new WorkEntryValidator();
        var workEntry = new WorkEntry(
            description,
            dateRange,
            Guid.NewGuid(),
            issueId);
        var validationResult = validator.Validate(workEntry);
        if (validationResult.IsValid)
        {
            return workEntry;
        }
        throw new ValidationException(validationResult.Errors);
    }

    private class WorkEntryValidator : AbstractValidator<WorkEntry>
    {
        public WorkEntryValidator()
        {
            RuleFor(x => x.Description).NotNull().WithMessage("Description must not be null!")
                                       .NotEmpty().WithMessage("Description must not be empty!");
            RuleFor(x => x.IssueId).NotNull().WithMessage("IssueId must not be null!");
        }
    }

}
