using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Infrastructure.Factories;

public static class LabelFactory
{
    public static Core.Models.Entities.Label MapToDomain(Models.Label label) =>
        Core.Models.Entities.Label.Create(
            label.Name,
            label.Description,
            label.Color,
            label.Id);
}
