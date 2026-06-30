using BaseLib.Utils;
using Crawler.Mod.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Crawler.Mod.Cards;

[Pool(typeof(CrawlerCardPool))]
public sealed class BloodTap() : CrawlerCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DrawCards(choiceContext, ApplyMultiplier(UpgradeValue(1, 2), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class SanguineGuard() : CrawlerCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await GainBlock(cardPlay, ApplyMultiplier(UpgradeValue(6, 9), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class MawStrike() : CrawlerCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(12, 16), multiplier));
        await ApplyDoom(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(1, 2), multiplier));
    }
}
