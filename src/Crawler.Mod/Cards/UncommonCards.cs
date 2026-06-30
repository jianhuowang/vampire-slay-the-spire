using BaseLib.Utils;
using Crawler.Mod.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Crawler.Mod.Cards;

[Pool(typeof(CrawlerCardPool))]
public sealed class HematicStep() : CrawlerCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await GainBlock(cardPlay, ApplyMultiplier(UpgradeValue(4, 6), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class BleedingLash() : CrawlerCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await DealDamage(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(9, 13), multiplier));
        await ApplyWeak(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(1, 2), multiplier));
    }
}

[Pool(typeof(CrawlerCardPool))]
public sealed class GraveBloom() : CrawlerCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var multiplier = ResolveChainMultiplier(cardPlay);

        await ApplyDoom(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(2, 3), multiplier));
        await DrawCards(choiceContext, ApplyMultiplier(UpgradeValue(1, 2), multiplier));
    }
}
