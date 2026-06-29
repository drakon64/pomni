using System.Text.Json;
using Pomni.Model;

namespace Pomni.Commands;

internal static class Modify
{
    public static void ModifyRepository(
        string name,
        Forge? forge = null,
        string? repository = null,
        string? branch = null,
        ReferenceType? type = null,
        bool frozen = false
    )
    {
        var pomniJson = JsonSerializer.Deserialize<PomniPins>(
            File.ReadAllText("pomni/pomni.json"),
            SourceGenerationContext.Default.PomniPins
        );

        var pin = pomniJson.Pins[name];

        if (forge is not null)
            pin.Forge = (Forge)forge;
        if (repository is not null)
            pin.Repository = repository;
        if (branch is not null)
            pin.Branch = branch;
        if (type is not null)
            pin.Type = type;

        pin.Frozen = frozen;

        using var pomniJsonFile = File.Open("pomni/pomni.json", FileMode.Create);

        pomniJsonFile.Write(
            JsonSerializer.SerializeToUtf8Bytes(
                pomniJson,
                SourceGenerationContext.Default.PomniPins
            )
        );
    }
}
