using Amazon.Runtime.Internal;
using GitHubSimulator.Core.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemoteBranch.Dtos
{
    public class GetGiteaBranchesDto
    {
        public string Name { get; set; }
        public PayloadCommit Commit { get; set; }
        public string Effective_Branch_Protection_Name { get; set; }
        public bool Enable_Status_Check { get; set; }
        public bool Protected { get; set; }
        public long Required_Approvals { get; set; }
        public string[] Status_Check_contexts { get; set;}
        public bool User_Can_Merge   { get; set; }
        public bool User_Can_Push { get; set; }

        public GetGiteaBranchesDto(string name, PayloadCommit commit, string effective_Branch_Protection_Name, 
            bool enable_Status_Check, bool @protected, long required_Approvals, string[] status_Check_contexts, bool user_Can_Merge, bool user_Can_Push)
        {
            Name = name;
            Commit = commit;
            Effective_Branch_Protection_Name = effective_Branch_Protection_Name;
            Enable_Status_Check = enable_Status_Check;
            Protected = @protected;
            Required_Approvals = required_Approvals;
            Status_Check_contexts = status_Check_contexts;
            User_Can_Merge = user_Can_Merge;
            User_Can_Push = user_Can_Push;
        }
    }

    public class PayloadCommit {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string[] Added { get; set; }
        public PayloadUser Author { get; set; }
        public PayloadUser Committer { get; set; }
        public string[] Modified { get; set; }
        public string[] Removed { get; set; }
        public DateTime Timestamp { get; set; }
        public PayloadCommitVerification Verification { get; set;}

        public PayloadCommit(string id, string message, string url, string[] added, PayloadUser author,
            PayloadUser committer, string[] modified, string[] removed, DateTime timestamp, PayloadCommitVerification verification)
        {
            Id = id;
            Message = message;
            Url = url;
            Added = added;
            Author = author;
            Committer = committer;
            Modified = modified;
            Removed = removed;
            Timestamp = timestamp;
            Verification = verification;
        }
    }

    public class PayloadCommitVerification { 
        public string Payload { get; set; }
        public string Reason { get; set; }
        public string Signature { get; set; }
        public PayloadUser Signer { get; set; }
        public bool Verified { get; set; }

        public PayloadCommitVerification(string payload, string reason, string signature, PayloadUser signer, bool verified)
        {
            Payload = payload;
            Reason = reason;
            Signature = signature;
            Signer = signer;
            Verified = verified;
        }
    }

    public class PayloadUser
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public PayloadUser(string email, string name, string username)
        {
            Email = email;
            Name = name;
            Username = username;
        }
    }
}
