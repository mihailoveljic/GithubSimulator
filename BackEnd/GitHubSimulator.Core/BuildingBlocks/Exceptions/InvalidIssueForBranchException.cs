namespace GitHubSimulator.Core.BuildingBlocks.Exceptions;

public class InvalidIssueForBranchException : Exception
{
    public InvalidIssueForBranchException() 
        : base("Error while creating the branch, issue not found")
    {}
}