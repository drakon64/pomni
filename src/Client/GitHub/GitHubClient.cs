using System.Text.Json.Serialization;

namespace Pomni.Client.GitHub;

internal static partial class GitHubClient
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("https://api.github.com/"),
        DefaultRequestHeaders =
        {
            { "Accept", "application/vnd.github+json" },
            { "User-Agent", "Pomni 0.0.1" },
            { "X-GitHub-Api-Version", "2026-03-10" },
        },
    };
}

[JsonSerializable(typeof(Repository))]
[JsonSerializable(typeof(Branch))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
internal partial class GitHubClientSourceGenerationContext : JsonSerializerContext;
