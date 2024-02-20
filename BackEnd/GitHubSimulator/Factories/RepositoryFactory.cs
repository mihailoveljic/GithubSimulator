using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Dtos.Repositories;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

namespace GitHubSimulator.Factories;

public class RepositoryFactory
{
    public Repository MapToDomain(InsertRepositoryDto dto, string owner) =>
        Repository.Create(
            dto.Name,
            dto.Description,
            dto.Visibility,
            owner
        );
    
    public Repository MapToDomain(UpdateRepositoryDto dto, string owner = "") =>
        Repository.Create(
            dto.Name,
            dto.Description,
            dto.Visibility,
            owner,
            dto.Id
        );

    public CreateGiteaRepositoryDto MapToGiteaDto(InsertRepositoryDto dto) =>
        new(dto.Name, dto.Description, dto.Visibility.ToString() == "Private", dto.Gitignores, dto.License);
}