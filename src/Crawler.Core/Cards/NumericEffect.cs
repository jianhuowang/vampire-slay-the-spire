namespace Crawler.Core.Cards;

public sealed record NumericEffect(string Key, int BaseValue, bool IsMultiplierEligible = true);
