using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Core.Models.Entities;

sealed class Label : Event
{
    public string Name { get; }

    private Label(
        string name,
        Guid id,
        DateTime dateTimeOccured) : base(id, dateTimeOccured, Enums.EventType.Label)
    {
        Name = name;
    }

    public static Label Create(string name)
    {
        var validator = new LabelValidator();
        var label = new Label(
            name,
            Guid.NewGuid(),
            DateTime.Now);
        var validatorResult = validator.Validate(label);
        if (validatorResult.IsValid)
            return label;

        throw new ValidationException(validatorResult.Errors);
    }

    private class LabelValidator : AbstractValidator<Label>
    {
        public LabelValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Label must not be null!")
                                .NotEmpty().WithMessage("Label must not be empty!");
        }
    }
}
