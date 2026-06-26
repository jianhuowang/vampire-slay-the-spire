using Crawler.Mod.Adapters;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Crawler.Mod.Relics;

public sealed class TurboturnMeter() : CrawlerRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override bool ShowCounter => true;
    public override bool ShouldReceiveCombatHooks => true;
    public override int DisplayAmount => Owner is null
        ? 1
        : Math.Max(1, CrawlerChainRuntime.GetCurrentState(Owner).Multiplier);

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        CrawlerChainRuntime.ResetTurn(player);
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        InvokeDisplayAmountChanged();
        Flash();
        return Task.CompletedTask;
    }
}
