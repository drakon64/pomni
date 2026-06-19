using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands;

internal static class Init
{
    public static void InitPomniJson()
    {
        using var pomniJson = File.Open("pomni.json", FileMode.CreateNew);
        using var pomniLockJson = File.Open("pomni.lock.json", FileMode.CreateNew);

        pomniJson.Write(
            JsonSerializer.SerializeToUtf8Bytes<Dictionary<string, PomniPin>>(
                new Dictionary<string, PomniPin>(),
                SourceGenerationContext.Default.DictionaryStringPomniPin
            )
        );

        pomniLockJson.Write(
            JsonSerializer.SerializeToUtf8Bytes<Dictionary<string, PomniLock>>(
                new Dictionary<string, PomniLock>(),
                SourceGenerationContext.Default.DictionaryStringPomniLock
            )
        );
    }
}
