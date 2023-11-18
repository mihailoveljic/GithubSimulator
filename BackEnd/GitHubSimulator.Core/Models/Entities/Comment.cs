using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Core.Models.Entities;

sealed class Comment : Event
{
    public string Content { get; }
    public Guid TaskId { get; }

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
        Guid taskId)
    {
        var validator = new CommentValidator();

        var comment = new Comment(
            Guid.NewGuid(),
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
            RuleFor(x => x.TaskId).NotNull().WithMessage("Comment content must not be null!")
                                  .NotEmpty().WithMessage("Comment content must not be empty!");
        }
    }
}
