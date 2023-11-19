﻿using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.AggregateRoots;

sealed class Repository : Entity
{
    public string Name { get; }
    public string Description { get; }

    private Repository(Guid id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public static Repository Create(string name, string description)
    {
        var validator = new RepositoryValidator();
        var repository = new Repository(
            Guid.NewGuid(),
            name,
            description);
        var validationResult = validator.Validate(repository);
        if (validationResult.IsValid)
        {
            return repository;
        }
        throw new ValidationException(validationResult.Errors);
    }

    private class RepositoryValidator : AbstractValidator<Repository>
    {
        public RepositoryValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name must not be null!")
                                .NotEmpty().WithMessage("Name must not be empty!");
            RuleFor(x => x.Description).NotNull().WithMessage("Description must not be null!")
                                       .NotEmpty().WithMessage("Description must not be empty!");
        }
    }
}
