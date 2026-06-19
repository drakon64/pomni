using System.Net.Http.Json;

namespace Pomni.Client.GitHub;

internal class Repository
{
    public required string DefaultBranch { get; init; }
}

internal partial class GitHubClient
{
    public static async Task<Repository> GetRepository(string repo)
    {
        var request = await HttpClient.SendAsync(
            new HttpRequestMessage { RequestUri = new Uri($"repos/{repo}", UriKind.Relative) }
        );

        var response = await request.Content.ReadFromJsonAsync<Repository>(
            GitHubClientSourceGenerationContext.Default.Repository
        );

        return response;
    }
}
