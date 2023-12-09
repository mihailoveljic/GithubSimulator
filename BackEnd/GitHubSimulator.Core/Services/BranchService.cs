using CSharpFunctionalExtensions;
using GitHubSimulator.Core.BuildingBlocks.Exceptions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Services;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _branchRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IIssueRepository _issueRepository;

    public BranchService(IBranchRepository branchRepository, IRepositoryRepository repositoryRepository, IIssueRepository issueRepository)
    {
        _branchRepository = branchRepository;
        _repositoryRepository = repositoryRepository;
        _issueRepository = issueRepository;
    }

    public async Task<Branch> GetById(Guid id)
    {
        return await _branchRepository.GetById(id);
    }

    public async Task<IEnumerable<Branch>> GetAll()
    {
        return await _branchRepository.GetAll();
    }

    public async Task<Branch> Insert(Branch branch)
    {
        await CheckIfRepositoryExists(branch.RepositoryId);

        if (branch.IssueId is null) return await _branchRepository.Insert(branch);
        
        await CheckIfIssueExists((Guid)branch.IssueId);
        return await _branchRepository.Insert(branch);
    }

    public async Task<Maybe<Branch>> Update(Branch branch)
    {
        if (branch.IssueId is null) return await _branchRepository.Update(branch);
        
        await CheckIfIssueExists((Guid)branch.IssueId);
        return await _branchRepository.Update(branch);
    }

    private async Task CheckIfIssueExists(Guid branchId)
    {
        var issue = await _issueRepository.GetById(branchId);
        if (issue is null)
        {
            throw new InvalidIssueForBranchException();
        }
    }

    private async Task CheckIfRepositoryExists(Guid branchId)
    {
        var repository = await _repositoryRepository.GetById(branchId);
        if (repository is null)
        {
            throw new InvalidRepositoryForBranchException();
        }
    }
    
    public async Task<bool> Delete(Guid id)
    {
        return await _branchRepository.Delete(id);
    }
}