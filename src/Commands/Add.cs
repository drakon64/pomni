using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands;

internal static class Add
{
    public static void AddRepository(
        string name,
        Forge forge,
        string repository,
        string? reference,
        ReferenceType? referenceType,
        bool? frozen
    )
    {
        var pomniJson = JsonSerializer.Deserialize<Dictionary<string, PomniPin>>(
            File.ReadAllText("pomni.json"),
            SourceGenerationContext.Default.DictionaryStringPomniPin
        );

        pomniJson.Add(
            name,
            new PomniPin
            {
                Forge = forge,
                Repository = repository,
                Reference = reference,
                ReferenceType = referenceType,
                Frozen = frozen,
            }
        );

        using var pomniJsonFile = File.Open("pomni.json", FileMode.Create);

        pomniJsonFile.Write(
            JsonSerializer.SerializeToUtf8Bytes(
                pomniJson,
                SourceGenerationContext.Default.DictionaryStringPomniPin
            )
        );
    }
}
