namespace GitHubSimulator.Core.BuildingBlocks.Exceptions;

public class InvalidTaskForCommentException : Exception
{
    public InvalidTaskForCommentException() : base("No tasks associated with the comment found")
    {}
}