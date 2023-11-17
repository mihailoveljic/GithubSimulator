using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.ValueObjects;

public class Label : ValueObject
{
    public string Name { get; init; } = null!;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }

    private Label(string name)
    {
        Name = name;
    }

    public static Label Create(string name)
    {
        var validator = new LabelValidator();
        var label = new Label(name);
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
                                .NotEmpty().WithMessage("Email must not be empty!");
        }
    }
}
