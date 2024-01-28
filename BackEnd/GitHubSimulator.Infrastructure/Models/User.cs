using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Infrastructure.Models;
public class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Role { get; set; }
    public string MailEmail { get; set; }
    public string AccountCredentialsUsername { get; set; }
    public string AccountCredentialsPasswordHash { get; set; }
}
