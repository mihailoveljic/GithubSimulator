using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Core.Models.Entities;

public sealed class Comment : Event
{
    public string Content { get; init; }
    public Guid TaskId { get; init; }

    private Comment(
        Guid id, 
        DateTime dateTimeOccured,
        string content,
        Guid taskId) : base(id, dateTimeOccured, Enums.EventType.Comment)
    {
        Content = content;
        TaskId = taskId;
    }

    public static Comment Create(
        string content,
        Guid taskId,
        Guid? id = null)
    {
        var validator = new CommentValidator();

        var comment = new Comment(
            id ?? Guid.NewGuid(),
            DateTime.Now,
            content,
            taskId);

        var validatorResult = validator.Validate(comment);

        if (validatorResult.IsValid)
            return comment;

        throw new ValidationException(validatorResult.Errors);
    }

    private class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Content).NotNull().WithMessage("Comment content must not be null!")
                                   .NotEmpty().WithMessage("Comment content must not be empty!");
            RuleFor(x => x.TaskId)
                .NotNull().WithMessage("The task associated with the comment must not be null!")
                .NotEmpty().WithMessage("The task associated with the comment must not be empty!");
        }
    }
}
