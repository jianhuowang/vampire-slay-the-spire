namespace Crawler.Core.Cards;

public sealed record CardDefinition(
    string Id,
    string Name,
    int Cost,
    string Route,
    string Rarity,
    IReadOnlyList<NumericEffect> Effects,
    IReadOnlyList<string> Tags);
