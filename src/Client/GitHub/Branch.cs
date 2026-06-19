using System.Net.Http.Json;

namespace Pomni.Client.GitHub;

internal class Branch
{
    public required Commit Commit { get; init; }
}

internal class Commit
{
    public required string Sha { get; init; }
}

internal partial class GitHubClient
{
    public static async Task<Branch> GetBranch(string repo, string branch)
    {
        var request = await HttpClient.SendAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri($"repos/{repo}/branches/{branch}", UriKind.Relative),
            }
        );

        var response = await request.Content.ReadFromJsonAsync<Branch>(
            GitHubClientSourceGenerationContext.Default.Branch
        );

        return response;
    }
}
