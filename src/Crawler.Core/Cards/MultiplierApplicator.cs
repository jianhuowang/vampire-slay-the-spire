namespace Crawler.Core.Cards;

public static class MultiplierApplicator
{
    public static IReadOnlyDictionary<string, int> Apply(CardDefinition card, int multiplier)
    {
        ArgumentNullException.ThrowIfNull(card);

        if (multiplier < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be at least 1.");
        }

        return card.Effects.ToDictionary(
            effect => effect.Key,
            effect => effect.IsMultiplierEligible ? effect.BaseValue * multiplier : effect.BaseValue);
    }
}
