using System.Text.Json.Serialization;

namespace Pomni.Model;

internal class PomniPins
{
    public required byte Version { get; init; }
    public required Dictionary<string, PomniPin> Pins { get; init; }
}

internal class PomniPin
{
    public required Forge Forge { get; init; }
    public required string Repository { get; init; }

    public string? Branch { get; init; }
    public ReferenceType? Type { get; init; }
    public bool? Frozen { get; init; }
}

internal enum Forge
{
    [JsonStringEnumMemberName("github")]
    GitHub,
}

internal enum ReferenceType
{
    [JsonStringEnumMemberName("branch")]
    Branch,

    [JsonStringEnumMemberName("release")]
    Release,
}
