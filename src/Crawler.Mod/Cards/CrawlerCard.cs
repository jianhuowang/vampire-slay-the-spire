using BaseLib.Abstracts;
using BaseLib.Utils;
using Crawler.Mod.Character;
using Crawler.Mod.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crawler.Mod.Cards;

[Pool(typeof(CrawlerCardPool))]
public abstract class CrawlerCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    public override string CustomPortraitPath => "card.png".BigCardImagePath();
    public override string PortraitPath => "card.png".CardImagePath();
    public override string BetaPortraitPath => "card.png".CardImagePath();

    protected Task DealDamage(PlayerChoiceContext choiceContext, CardPlay cardPlay, decimal damage)
    {
        return CreatureCmd.Damage(
            choiceContext,
            RequireTarget(cardPlay),
            new DamageVar(damage, ValueProp.Unpowered),
            Owner.Creature,
            this);
    }

    protected Task GainBlock(CardPlay cardPlay, decimal block)
    {
        return CreatureCmd.GainBlock(
            Owner.Creature,
            new BlockVar(block, ValueProp.Unpowered),
            cardPlay,
            fast: false);
    }

    protected Task DrawCards(PlayerChoiceContext choiceContext, decimal count)
    {
        return CardPileCmd.Draw(choiceContext, count, Owner, fromHandDraw: false);
    }

    protected Task ApplyWeak(PlayerChoiceContext choiceContext, CardPlay cardPlay, decimal amount)
    {
        return PowerCmd.Apply<WeakPower>(
            choiceContext,
            RequireTarget(cardPlay),
            amount,
            Owner.Creature,
            this,
            silent: false);
    }

    protected Task ApplyDoom(PlayerChoiceContext choiceContext, CardPlay cardPlay, decimal amount)
    {
        return PowerCmd.Apply<DoomPower>(
            choiceContext,
            RequireTarget(cardPlay),
            amount,
            Owner.Creature,
            this,
            silent: false);
    }

    private static Creature RequireTarget(CardPlay cardPlay)
    {
        return cardPlay.Target ?? throw new InvalidOperationException("This Crawler card requires a target.");
    }
}
