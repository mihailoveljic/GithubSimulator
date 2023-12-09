using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IBranchService
{
    Task<Branch> GetById(Guid id);
    Task<IEnumerable<Branch>> GetAll();
    Task<Branch> Insert(Branch branch);
    Task<Maybe<Branch>> Update(Branch branch);
    Task<bool> Delete(Guid id);
}