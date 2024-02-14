namespace GitHubSimulator.Infrastructure.RemoteRepository.Dtos
{
    public record CreateGiteaRepositoryDto(
        string Name,
        string Description,
        bool Private,
        string Gitignores,
        string License,
        string Default_Branch = "main");
}
