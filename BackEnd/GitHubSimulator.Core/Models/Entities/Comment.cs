using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Core.Models.Entities;

sealed class Comment : Event
{
    public string Content { get; }

    private Comment(
        Guid id, 
        DateTime dateTimeOccured,
        string content) : base(id, dateTimeOccured)
    {
        Content = content;
    }

    public static Comment Create(
        string content)
    {
        var validator = new CommentValidator();

        var comment = new Comment(
            Guid.NewGuid(),
            DateTime.Now,
            content);

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
        }
    }
}
