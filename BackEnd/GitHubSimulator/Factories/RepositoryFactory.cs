using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Dtos.Repositories;

namespace GitHubSimulator.Factories;

public class RepositoryFactory
{
    public Repository MapToDomain(InsertRepositoryDto dto) =>
        Repository.Create(
            dto.Name,
            dto.Description,
            dto.Visibility
        );
    
    public Repository MapToDomain(UpdateRepositoryDto dto) =>
        Repository.Create(
            dto.Name,
            dto.Description,
            dto.Visibility,
            dto.Id
        );
}