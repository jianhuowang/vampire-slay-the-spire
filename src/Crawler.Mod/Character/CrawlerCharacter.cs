using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Crawler.Mod.Cards;
using Crawler.Mod.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Crawler.Mod.Character;

public sealed class CrawlerCharacter : PlaceholderCharacterModel
{
    public const string CharacterId = "VampireCrawler.TheCrawler";

    public static readonly Color Color = new("9f263f");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<QuickStab>(),
        ModelDb.Card<QuickStab>(),
        ModelDb.Card<WhipCrack>(),
        ModelDb.Card<WhipCrack>(),
        ModelDb.Card<GuardedDash>(),
        ModelDb.Card<GuardedDash>(),
        ModelDb.Card<HeavySwing>(),
        ModelDb.Card<PocketWatch>(),
        ModelDb.Card<BadOmen>(),
        ModelDb.Card<BadOmen>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningBlood>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<CrawlerCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<CrawlerRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<CrawlerPotionPool>();

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}
