using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Middlewares;

public static class RepositoryAuthorizationMiddleware
{
    private static readonly Dictionary<string, List<UserRepositoryRole>> MethodPermissions 
        = new()
        {
            { "ReadIssues", new List<UserRepositoryRole> { UserRepositoryRole.Admin, UserRepositoryRole.Owner, 
                UserRepositoryRole.Read, UserRepositoryRole.Triage, UserRepositoryRole.Write}},
            { "ManageIssues", new List<UserRepositoryRole>
            {
                UserRepositoryRole.Admin, UserRepositoryRole.Owner,
                UserRepositoryRole.Triage, UserRepositoryRole.Write
            }},
            { "ManageRepositorySettings", new List<UserRepositoryRole>
            {
                UserRepositoryRole.Admin, UserRepositoryRole.Owner
            }},
            { "DeleteRepository", new List<UserRepositoryRole>
            {
                UserRepositoryRole.Owner
            }},
            { "TransferOwnership", new List<UserRepositoryRole>
            {
                UserRepositoryRole.Owner
            }},
        };
    
    public static bool Authorize(UserRepository userRepository, string action)
    {
        if (MethodPermissions.TryGetValue(action, out var allowedRoles))
        {
            return allowedRoles.Contains(userRepository.UserRepositoryRole);
        }

        return false;
    }
}