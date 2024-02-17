namespace GitHubSimulator.Infrastructure.RemoteRepository.Dtos
{
    public record GiteaDocumentDto(
        string Name,
        string Type,
        string Content,
        string Last_Commit_Sha,
        double Size,
        string Download_Url
        );
}
