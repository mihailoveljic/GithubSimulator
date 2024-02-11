namespace GitHubSimulator.Infrastructure.RemoteRepository.Dtos
{
    public record GiteaRepositoryDto(
        string Name,
        string Description,
        bool Private,
        string Gitignores,
        string License,
        string Default_Branch = "main");
}
