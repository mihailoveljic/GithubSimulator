using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Dtos.Repositories;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

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

    public CreateGiteaRepositoryDto MapToGiteaDto(InsertRepositoryDto dto) =>
        new(dto.Name, dto.Description, dto.Visibility.ToString() == "Private", dto.Gitignores, dto.License);
}