namespace Crawler.Core.Cards;

public sealed record CardDefinition(
    string Id,
    string Name,
    int Cost,
    string Route,
    IReadOnlyList<NumericEffect> Effects,
    IReadOnlyList<string> Tags);
