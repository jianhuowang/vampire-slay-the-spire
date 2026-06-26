using System.Text.Json;
using Crawler.Core.Cards;

namespace Crawler.Content;

public static class CardCatalog
{
    public static IReadOnlyList<CardDefinition> LoadV1()
    {
        var assembly = typeof(CardCatalog).Assembly;
        const string resourceName = "Crawler.Content.cards.v1.json";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        var records = JsonSerializer.Deserialize<List<CardRecord>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Card catalog could not be deserialized.");

        return records.Select(record => new CardDefinition(
            record.Id,
            record.Name,
            record.Cost,
            record.Route,
            record.Effects.Select(effect => new NumericEffect(effect.Key, effect.BaseValue, effect.IsMultiplierEligible)).ToArray(),
            record.Tags)).ToArray();
    }

    private sealed record CardRecord(
        string Id,
        string Name,
        int Cost,
        string Route,
        string Rarity,
        IReadOnlyList<EffectRecord> Effects,
        IReadOnlyList<string> Tags);

    private sealed record EffectRecord(string Key, int BaseValue, bool IsMultiplierEligible = true);
}
