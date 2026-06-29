using System.Diagnostics;
using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands.Update;

internal static partial class Update
{
    public static async Task UpdateRepositories()
    {
        await using var pomniJson = File.OpenRead("pomni/pomni.json");
        var pomniLockJson = File.OpenRead("pomni/pomni.lock.json");

        var pomniPins = await JsonSerializer.DeserializeAsync<PomniPins>(
            pomniJson,
            SourceGenerationContext.Default.PomniPins
        );

        var pomniLocks = await JsonSerializer.DeserializeAsync<Dictionary<string, PomniLock>>(
            pomniLockJson,
            SourceGenerationContext.Default.DictionaryStringPomniLock
        );

        await pomniLockJson.DisposeAsync();

        var updatedLocks = new Dictionary<string, PomniLock>();

        foreach (var pin in pomniPins.Pins)
        {
            if (!pin.Value.Frozen)
            {
                var newLock = pomniLocks.GetValueOrDefault(pin.Key);

                var updatedPin = pin.Value.Forge switch
                {
                    Forge.GitHub => await UpdateGitHubRepository(pin.Value),
                };

                if (newLock is not null)
                    updatedLocks[pin.Key] = updatedPin;
                else
                    updatedLocks.Add(pin.Key, updatedPin);
            }
            else
                updatedLocks[pin.Key] = pomniLocks[pin.Key];
        }

        pomniLockJson = File.Open("pomni/pomni.lock.json", FileMode.Truncate);
        await pomniLockJson.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(
                updatedLocks,
                SourceGenerationContext.Default.DictionaryStringPomniLock
            )
        );
        await pomniLockJson.DisposeAsync();
    }

    private static async Task<string> GetSri(string url)
    {
        var prefetchProcessStartInfo = new ProcessStartInfo
        {
            FileName = "nix-prefetch-url",
            ArgumentList = { url, "--unpack" },
            RedirectStandardOutput = true,
        };

        var prefetchProcess = Process.Start(prefetchProcessStartInfo);

        await prefetchProcess.WaitForExitAsync();

        var prefetch = (await prefetchProcess.StandardOutput.ReadToEndAsync()).TrimEnd();

        var convertProcessStartInfo = new ProcessStartInfo
        {
            FileName = "nix-hash",
            ArgumentList = { "--type", "sha256", "--to-sri", prefetch },
            RedirectStandardOutput = true,
        };

        var convertProcess = Process.Start(convertProcessStartInfo);

        var stdout = await convertProcess.StandardOutput.ReadToEndAsync();

        return stdout.TrimEnd('\n');
    }
}
