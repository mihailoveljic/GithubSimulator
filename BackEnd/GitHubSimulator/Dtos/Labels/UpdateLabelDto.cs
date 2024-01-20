namespace GitHubSimulator.Dtos.Labels;

public record UpdateLabelDto(Guid Id, string Name, string Description, string Color);
