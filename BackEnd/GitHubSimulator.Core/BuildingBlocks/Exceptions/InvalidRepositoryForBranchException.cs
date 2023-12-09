namespace GitHubSimulator.Core.BuildingBlocks.Exceptions;

public class InvalidRepositoryForBranchException : Exception
{
    public InvalidRepositoryForBranchException() 
        : base("Error while creating the branch, repository not found")
    {}
}