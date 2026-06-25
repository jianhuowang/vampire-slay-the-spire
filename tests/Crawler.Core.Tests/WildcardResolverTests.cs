using Crawler.Core.Wildcards;

namespace Crawler.Core.Tests;

public sealed class WildcardResolverTests
{
    [Fact]
    public void TreatAsNextCostUsesPreviousCostPlusOne()
    {
        var context = new WildcardContext(LastPlayedCost: 2, CurrentMultiplier: 3, PrintedPlayedCost: 0);
        var rules = new[] { new WildcardRule(WildcardKind.TreatAsNextCost) };

        var result = WildcardResolver.Resolve(context, rules);

        Assert.Equal(3, result.EffectivePlayedCost);
        Assert.False(result.PreserveMultiplierOnMiss);
    }

    [Fact]
    public void CopyPreviousCostUsesPreviousCostWhenAvailable()
    {
        var context = new WildcardContext(LastPlayedCost: 1, CurrentMultiplier: 2, PrintedPlayedCost: 3);
        var rules = new[] { new WildcardRule(WildcardKind.CopyPreviousCost) };

        var result = WildcardResolver.Resolve(context, rules);

        Assert.Equal(1, result.EffectivePlayedCost);
    }

    [Fact]
    public void PreserveMultiplierFlagCanBeCarriedToAdapter()
    {
        var context = new WildcardContext(LastPlayedCost: 1, CurrentMultiplier: 2, PrintedPlayedCost: 3);
        var rules = new[] { new WildcardRule(WildcardKind.PreserveMultiplierOnMiss) };

        var result = WildcardResolver.Resolve(context, rules);

        Assert.Equal(3, result.EffectivePlayedCost);
        Assert.True(result.PreserveMultiplierOnMiss);
    }
}
