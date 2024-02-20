using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemotePullRequest.Dtos
{
    public class GiteaCommitDto
    {
        public string Sha { get; set; }
        public string Created { get; set; }
        public CommitDto Commit { get; set; }
        public File[] Files { get; set; }
        public Stats Stats { get; set; }
        public Parent[] Parents { get; set; }

        public GiteaCommitDto(string sha, string created, CommitDto commit, File[] files, Stats stats, Parent[] parents)
        {
            Sha = sha;
            Created = created;
            Commit = commit;
            Files = files;
            Stats = stats;
            Parents = parents;
        }
    }

    public class CommitDto { 
        public string Message { get; set; }
        public Parent Tree { get; set; }

        public CommitDto(string message, Parent tree)
        {
            Message = message;
            Tree = tree;
        }
    }

    public class Parent { 
        public string Sha { get; set; }
        public string Created { get; set; }

        public Parent(string sha, string created)
        {
            Sha = sha;
            Created = created;
        }
    }

    public class File {
        public File(string fileName, string status)
        {
            FileName = fileName;
            Status = status;
        }

        public string FileName { get; set; }
        public string Status { get; set; }

    }
    public class Stats { 
        public int Total { get; set; }
        public int Additions { get; set; }
        public int Deletions { get; set; }

        public Stats(int total, int additions, int deletions)
        {
            Total = total;
            Additions = additions;
            Deletions = deletions;
        }
    }
}
