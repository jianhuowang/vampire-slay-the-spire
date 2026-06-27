namespace Crawler.Mod.Adapters;

public sealed class ChainCombatAdapter
{
    private CostChainState _state = CostChainEngine.StartTurn();

    public CostChainState CurrentState => _state;

    public void StartTurn()
    {
        _state = CostChainEngine.StartTurn();
    }

    public ChainResolution ResolvePlayerPlayedCard(int actualPlayedCost)
    {
        var result = CostChainEngine.ResolvePlayedCard(_state, actualPlayedCost);
        _state = result.NextState;
        return result;
    }
}
