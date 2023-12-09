using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;
public interface ILabelService
{
    Task<IEnumerable<Label>> GetAll();
    Task<Label> Insert(Label label);
    Task<Maybe<Label>> Update(Label label);
    Task<bool> Delete(Guid id);
}
