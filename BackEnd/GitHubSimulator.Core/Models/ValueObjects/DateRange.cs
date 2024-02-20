using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.ValueObjects;

sealed class DateRange : ValueObject
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }

    private DateRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }

    public static DateRange Create(DateTime start, DateTime end)
    {
        var validator = new DateRangeValidator();
        var dateRange = new DateRange(start, end);
        var validationResult = validator.Validate(dateRange);
        if (validationResult.IsValid)
        {
            return dateRange;
        }
        throw new ValidationException(validationResult.Errors);
    }

    public double GetDurationInHours()
    {
        TimeSpan duration = End - Start;
        return duration.TotalHours;
    }

    private class DateRangeValidator : AbstractValidator<DateRange>
    {
        public DateRangeValidator()
        {
            RuleFor(x => x.Start).NotNull().WithMessage("Start Date must be defined!");
            RuleFor(x => x.End).NotNull().WithMessage("End Date must be defined!");
            RuleFor(dateRange => dateRange.Start)
                                .LessThan(dateRange => dateRange.End)
                                .WithMessage("Start date must be before the end date.");
        }
    }
}
