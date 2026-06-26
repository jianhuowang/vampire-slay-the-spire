using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Crawler.Mod.Cards;

public sealed class QuickStab() : CrawlerCard(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(5, multiplier));
    }
}

public sealed class WhipCrack() : CrawlerCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(8, multiplier));
        await ApplyWeak(choiceContext, cardPlay, ApplyMultiplier(1, multiplier));
    }
}

public sealed class GuardedDash() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await GainBlock(cardPlay, ApplyMultiplier(7, multiplier));
    }
}

public sealed class HeavySwing() : CrawlerCard(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(14, multiplier));
    }
}

public sealed class PocketWatch() : CrawlerCard(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DrawCards(choiceContext, ApplyMultiplier(1, multiplier));
    }
}

public sealed class BadOmen() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await ApplyDoom(choiceContext, cardPlay, ApplyMultiplier(1, multiplier));
    }
}
