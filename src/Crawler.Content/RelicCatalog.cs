using System.Reflection;
using System.Text.Json;

namespace Crawler.Content;

public sealed record RelicDefinition(
    string Id,
    string Name,
    string Rarity,
    string Description,
    IReadOnlyList<string> Tags);

public static class RelicCatalog
{
    public static IReadOnlyList<RelicDefinition> LoadV1()
    {
        var assembly = typeof(RelicCatalog).Assembly;
        const string resourceName = "Crawler.Content.relics.v1.json";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        return JsonSerializer.Deserialize<List<RelicDefinition>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Relic catalog could not be deserialized.");
    }
}
