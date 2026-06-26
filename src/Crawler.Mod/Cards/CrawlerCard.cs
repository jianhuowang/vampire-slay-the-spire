using BaseLib.Abstracts;
using BaseLib.Utils;
using Crawler.Mod.Character;
using Crawler.Mod.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace Crawler.Mod.Cards;

[Pool(typeof(CrawlerCardPool))]
public abstract class CrawlerCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    public override string CustomPortraitPath => "card.png".BigCardImagePath();
    public override string PortraitPath => "card.png".CardImagePath();
    public override string BetaPortraitPath => "card.png".CardImagePath();
}
