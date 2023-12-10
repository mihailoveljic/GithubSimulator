namespace GitHubSimulator.Dtos.Comments;

public record UpdateCommentDto(
    string Content,
    Guid Id
);