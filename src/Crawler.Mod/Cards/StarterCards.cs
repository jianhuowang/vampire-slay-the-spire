using BaseLib.Utils;
using Crawler.Mod.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Crawler.Mod.Cards;

[Pool(typeof(CrawlerCardPool))]
public sealed class QuickStab() : CrawlerCard(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(5, 7), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class WhipCrack() : CrawlerCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(8, 10), multiplier));
        await ApplyWeak(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(1, 2), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class GuardedDash() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await GainBlock(cardPlay, ApplyMultiplier(UpgradeValue(7, 10), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class HeavySwing() : CrawlerCard(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(14, 18), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class PocketWatch() : CrawlerCard(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DrawCards(choiceContext, ApplyMultiplier(UpgradeValue(1, 2), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class BadOmen() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await ApplyDoom(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(1, 2), multiplier));
    }
}
