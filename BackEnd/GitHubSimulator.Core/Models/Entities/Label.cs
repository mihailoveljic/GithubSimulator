using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Core.Models.Entities;

public sealed class Label : Event
{
    public string Name { get; init; }

    public string Description { get; init; }

    public string Color { get; init; }

    private Label(
        string name,
        string description,
        string color,
        Guid id,
        DateTime dateTimeOccured) : base(id, dateTimeOccured, Enums.EventType.Label)
    {
        Name = name;
        Description = description;
        Color = color;
    }

    public static Label Create(string name, string description, string color, Guid? id = null)
    {
        var validator = new LabelValidator();
        var label = new Label(
            name,
            description,
            color,
            id is null ? Guid.NewGuid() : id.Value,
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
