namespace GitHubSimulator.Infrastructure.Factories;
public static class CommentFactory
{
    public static Core.Models.Entities.Comment MapToDomain(Models.Comment comment) =>
        Core.Models.Entities.Comment.Create(
            comment.Content,
            comment.TaskId,
            comment.Id);
}
