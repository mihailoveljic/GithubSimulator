using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemotePullRequest.Dtos
{
    public record CreateGiteaPullRequest(
        string? Assignee,
        string Base,
        string Body,
        string Head,
        string Title,
        string[] Assignees = null
        );
}
