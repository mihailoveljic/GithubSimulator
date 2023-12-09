﻿using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Issues;
using GitHubSimulator.Dtos.Labels;

namespace GitHubSimulator.Factories;

public class LabelFactory
{
    public Label MapToDomain(InsertLabelDto dto) =>
        Label.Create(dto.Name);

    public Label MapToDomain(UpdateLabelDto dto) =>
        Label.Create(dto.Name, dto.Id);
}
