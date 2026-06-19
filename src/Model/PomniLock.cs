namespace Pomni.Model;

internal class PomniLock
{
    public required string Url { get; init; }
    public required string Revision { get; init; }
    public required string Hash { get; init; }
}
