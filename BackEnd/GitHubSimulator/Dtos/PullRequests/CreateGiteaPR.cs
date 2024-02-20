using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Dtos.PullRequests
{
    public class CreateGiteaPR
    {
        public string UpdateIssueAssigneeDto { get; set; }
        public string Assignee { get; set; }
        public string[] Assignees { get; set; }
        public string Base { get; set; }
        public string Body { get; set; }
        public string Head { get; set; }
        public string Title { get; set; }
    }
}
