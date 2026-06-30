using Crawler.Core.Chains;

namespace Crawler.Core.Tests;

public sealed class CostChainEngineTests
{
    [Fact]
    public void FirstCardStartsChainAtOneMultiplier()
    {
        var state = CostChainEngine.StartTurn();

        var result = CostChainEngine.ResolvePlayedCard(state, playedCost: 1);

        Assert.Equal(1, result.AppliedMultiplier);
        Assert.Equal(1, result.NextState.LastPlayedCost);
        Assert.Equal(1, result.NextState.Multiplier);
        Assert.False(result.ContinuedChain);
    }

    [Fact]
    public void ExactNextCostContinuesChainAndIncreasesMultiplier()
    {
        var state = CostChainEngine.ResolvePlayedCard(CostChainEngine.StartTurn(), 0).NextState;

        var result = CostChainEngine.ResolvePlayedCard(state, playedCost: 1);

        Assert.Equal(2, result.AppliedMultiplier);
        Assert.Equal(1, result.NextState.LastPlayedCost);
        Assert.Equal(2, result.NextState.Multiplier);
        Assert.True(result.ContinuedChain);
    }

    [Fact]
    public void NonMatchingCostResolvesAtOneAndRestartsChain()
    {
        var state = CostChainEngine.ResolvePlayedCard(CostChainEngine.StartTurn(), 0).NextState;
        state = CostChainEngine.ResolvePlayedCard(state, 1).NextState;

        var result = CostChainEngine.ResolvePlayedCard(state, playedCost: 1);

        Assert.Equal(1, result.AppliedMultiplier);
        Assert.Equal(1, result.NextState.LastPlayedCost);
        Assert.Equal(1, result.NextState.Multiplier);
        Assert.False(result.ContinuedChain);
    }
}
