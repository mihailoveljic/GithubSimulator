namespace GitHubSimulator.Infrastructure.Factories;
public static class RepositoryFactory
{
    public static Core.Models.AggregateRoots.Repository MapToDomain(Models.Repository repository) =>
        Core.Models.AggregateRoots.Repository.Create(
            repository.Name, 
            repository.Description, 
            repository.Visibility, 
            repository.Owner,
            repository.Id);
}
