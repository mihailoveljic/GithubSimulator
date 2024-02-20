using GitHubSimulator.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemotePullRequest.Dtos
{
    public class GiteaPullRequestDto
    {
        public long Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string State { get; set; }
        public bool Mergeable { get; set; }
        public bool Merged { get; set; }
        public string Merged_At { get; set; }
        public string Merged_Commit_Sha { get; set; }
        public string Merged_Base { get; set; }
        public string Created_At { get; set; }
        public string Update_At { get; set; }
        public string Closed_At { get; set; }
        public BranchDTO Head { get; set; }
        public BranchDTO Base { get; set; }

        public GiteaPullRequestDto(long id, int number, string title, 
            string body, string state, bool mergeable, bool merged, string merged_At, 
            string merged_Commit_Sha, string merged_Base, string created_At, string update_At,
            string closed_At, BranchDTO head, BranchDTO @base)
        {
            Id = id;
            Number = number;
            Title = title;
            Body = body;
            State = state;
            Mergeable = mergeable;
            Merged = merged;
            Merged_At = merged_At;
            Merged_Commit_Sha = merged_Commit_Sha;
            Merged_Base = merged_Base;
            Created_At = created_At;
            Update_At = update_At;
            Closed_At = closed_At;
            Head = head;
            Base = @base;
        }
    }

    public class BranchDTO {
        public BranchDTO(string @ref, int repo_Id, string sha)
        {
            Ref = @ref;
            Repo_Id = repo_Id;
            Sha = sha;
        }

        public string Ref { get; set; }
        public int Repo_Id { get; set; }
        public string Sha { get; set; }
 
    }
}
