using Crawler.Core.Cards;

namespace Crawler.Core.Tests;

public sealed class MultiplierApplicatorTests
{
    [Fact]
    public void MultipliesAllEligibleNumericEffects()
    {
        var card = new CardDefinition(
            "test_burst",
            "Burst",
            2,
            "Weapon",
            new[]
            {
                new NumericEffect("damage", 6),
                new NumericEffect("draw", 1),
                new NumericEffect("energy", 1)
            },
            Array.Empty<string>());

        var result = MultiplierApplicator.Apply(card, multiplier: 3);

        Assert.Equal(18, result["damage"]);
        Assert.Equal(3, result["draw"]);
        Assert.Equal(3, result["energy"]);
    }

    [Fact]
    public void LeavesExplicitlyIneligibleEffectsUnchanged()
    {
        var card = new CardDefinition(
            "test_fixed",
            "Fixed",
            1,
            "Item",
            new[] { new NumericEffect("createdCards", 2, IsMultiplierEligible: false) },
            Array.Empty<string>());

        var result = MultiplierApplicator.Apply(card, multiplier: 5);

        Assert.Equal(2, result["createdCards"]);
    }
}
