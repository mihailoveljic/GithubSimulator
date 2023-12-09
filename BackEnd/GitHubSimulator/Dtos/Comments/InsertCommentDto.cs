namespace GitHubSimulator.Dtos.Comments;

public record InsertCommentDto(
    string Content,
    Guid TaskId
);