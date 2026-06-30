using System.Collections.Concurrent;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Crawler.Mod.Adapters;

public static class CrawlerChainRuntime
{
    private static readonly ConcurrentDictionary<ulong, ChainCombatAdapter> ChainsByPlayer = new();

    public static ChainResolution ResolvePlayedCard(Player player, int playedCost)
    {
        return GetChain(player).ResolvePlayerPlayedCard(playedCost);
    }

    public static CostChainState GetCurrentState(Player player)
    {
        return GetChain(player).CurrentState;
    }

    public static void ResetTurn(Player player)
    {
        GetChain(player).StartTurn();
    }

    private static ChainCombatAdapter GetChain(Player player)
    {
        return ChainsByPlayer.GetOrAdd(player.NetId, _ => new ChainCombatAdapter());
    }
}
