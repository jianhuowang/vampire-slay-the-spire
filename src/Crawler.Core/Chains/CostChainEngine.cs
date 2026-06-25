namespace Crawler.Core.Chains;

public static class CostChainEngine
{
    public static CostChainState StartTurn() => new(null, 0);

    public static ChainResolution ResolvePlayedCard(CostChainState state, int playedCost)
    {
        if (playedCost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(playedCost), "Played cost cannot be negative.");
        }

        var continues = state.LastPlayedCost.HasValue && playedCost == state.LastPlayedCost.Value + 1;
        var appliedMultiplier = continues ? state.Multiplier + 1 : 1;
        var nextState = new CostChainState(playedCost, appliedMultiplier);

        return new ChainResolution(playedCost, appliedMultiplier, nextState, continues);
    }
}
