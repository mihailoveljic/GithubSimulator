﻿using FluentValidation;
using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.AggregateRoots;

public sealed class Repository : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Visibility Visibility { get; private set; }

    private Repository(
        Guid id, 
        string name, 
        string description,
        Visibility visibility) : base(id)
    {
        Name = name;
        Description = description;
        Visibility = visibility;
    }

    public static Repository Create(
        string name, 
        string description,
        Visibility visibility,
        Guid? id = null)
    {
        var validator = new RepositoryValidator();
        var repository = new Repository(
            id ?? Guid.NewGuid(),
            name,
            description,
            visibility);
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
            RuleFor(x => x.Visibility).NotNull().WithMessage("Visibility must be defined!");
        }
    }
}
