using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.Entities;

public class Issue : Entity, Abstractions.Task
{
    public string Title { get; }
    public string Description { get; }
    public DateTime CreatedAt { get; }
    public Mail Assigne { get; }

    private Issue(
        Guid id,
        string title, 
        string description, 
        DateTime createdAt, 
        Mail assigne) : base(id)
    {
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        Assigne = assigne;
    }

    public static Issue Create(
        string title,
        string description,
        Mail assigne)
    {
        var validator = new IssueValidator();

        var comment = new Issue(
            Guid.NewGuid(),
            title,
            description,
            DateTime.Now,
            assigne);

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
        }
    }
}
