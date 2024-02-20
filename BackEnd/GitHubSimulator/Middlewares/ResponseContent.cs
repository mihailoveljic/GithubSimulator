namespace GitHubSimulator.Middlewares;

public class ResponseContent
{
    public string Error { get; set; } = null!;
    public List<string>? Errors { get; set; }
}
