using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Repositories;

public interface ILabelRepository
{
    Task<IEnumerable<Label>> GetAll();
    Task<Label> Insert(Label label);
    Task<Maybe<Label>> Update(Label label);
    Task<bool> Delete(Guid id);
}
