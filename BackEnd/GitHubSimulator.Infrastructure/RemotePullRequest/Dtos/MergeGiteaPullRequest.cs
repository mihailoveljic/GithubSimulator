using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemotePullRequest.Dtos
{
   public record MergeGiteaPullRequest(
        string Do,
        string MergeMessageField,
        string MergeTitleField,
        string MergeCommitId,
        bool Delete_Branch_After_Merge,
        bool Force_Merge,
        string Head_Commit_Id,
        bool Merge_When_Checks_Succeed);

}
