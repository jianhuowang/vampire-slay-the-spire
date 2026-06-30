namespace Crawler.Core.Chains;

public sealed record CostChainState(int? LastPlayedCost, int Multiplier);

public sealed record ChainResolution(
    int PlayedCost,
    int AppliedMultiplier,
    CostChainState NextState,
    bool ContinuedChain);
