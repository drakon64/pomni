using Pomni.Client.GitHub;
using Pomni.Model;

namespace Pomni.Commands.Update;

internal partial class Update
{
    private static async Task<PomniLock> UpdateGitHubRepository(PomniPin pomniPin)
    {
        string sha;

        if (pomniPin.ReferenceType is ReferenceType.Branch or null)
        {
            var repo = pomniPin.Repository;
            string branch;

            if (pomniPin.Reference is not null)
            {
                branch = pomniPin.Reference;
            }
            else
            {
                var getRepo = await GitHubClient.GetRepository(repo);
                branch = getRepo.DefaultBranch;
            }

            var getBranch = await GitHubClient.GetBranch(repo, branch);
            sha = getBranch.Commit.Sha;
        }
        else
            sha = "";

        var url = $"https://github.com/{pomniPin.Repository}/archive/{sha}.tar.gz";

        return new PomniLock { Url = url, Hash = await GetSri(url) };
    }
}
