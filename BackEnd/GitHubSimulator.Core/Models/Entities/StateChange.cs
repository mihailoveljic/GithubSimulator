using FluentValidation;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Entities;

sealed class StateChange : Event
{
    public State OldState { get; init; }
    public State NewState { get; init; }

    private StateChange(
        Guid id, 
        DateTime dateTimeOccured,
        State oldState, 
        State newState) : base(id, dateTimeOccured, EventType.StateChange)
    {
        OldState = oldState;
        NewState = newState;
    }

    public static StateChange Create(
        State oldState,
        State newState)
    {
        var validator = new StateChangeValidator();

        var stateChange = new StateChange(
            Guid.NewGuid(), 
            DateTime.Now, 
            oldState, 
            newState);

        var validatorResult = validator.Validate(stateChange);

        if (validatorResult.IsValid)
            return stateChange;

        throw new ValidationException(validatorResult.Errors);
    }

    private class StateChangeValidator : AbstractValidator<StateChange>
    {
        public StateChangeValidator()
        {
            RuleFor(x => x.OldState).NotNull().WithMessage("Old State must not be null!");
            RuleFor(x => x.NewState).NotNull().WithMessage("New State must not be null!");
        }
    }
}
