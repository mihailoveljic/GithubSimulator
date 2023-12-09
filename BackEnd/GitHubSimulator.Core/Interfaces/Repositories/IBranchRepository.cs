using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Repositories;

public interface IBranchRepository
{
    Task<Branch> GetById(Guid id);
    Task<IEnumerable<Branch>> GetAll();
    Task<Branch> Insert(Branch branch);
    Task<Maybe<Branch>> Update(Branch updatedBranch);
    Task<bool> Delete(Guid id);
}