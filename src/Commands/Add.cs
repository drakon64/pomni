using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands;

internal static class Add
{
    public static void AddRepository(
        string name,
        Forge forge,
        string repository,
        string? branch,
        ReferenceType? type,
        bool? frozen
    )
    {
        var pomniJson = JsonSerializer.Deserialize<PomniPins>(
            File.ReadAllText("pomni/pomni.json"),
            SourceGenerationContext.Default.PomniPins
        );

        pomniJson.Pins.Add(
            name,
            new PomniPin
            {
                Forge = forge,
                Repository = repository,
                Branch = branch,
                Type = type,
                Frozen = frozen,
            }
        );

        using var pomniJsonFile = File.Open("pomni/pomni.json", FileMode.Create);

        pomniJsonFile.Write(
            JsonSerializer.SerializeToUtf8Bytes(
                pomniJson,
                SourceGenerationContext.Default.PomniPins
            )
        );
    }
}
