using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Comments;

namespace GitHubSimulator.Factories;

public class CommentFactory
{
    public Comment MapToDomain(InsertCommentDto dto) =>
        Comment.Create(
            dto.Content,
            dto.TaskId
        );

    public Comment MapToDomain(UpdateCommentDto dto) =>
        Comment.Create(
            dto.Content,
            Guid.NewGuid(),
            dto.Id
        );
}