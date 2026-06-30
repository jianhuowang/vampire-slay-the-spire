using BaseLib.Abstracts;
using Crawler.Mod.Extensions;
using Godot;

namespace Crawler.Mod.Character;

public sealed class CrawlerCardPool : CustomCardPoolModel
{
    public override string Title => CrawlerCharacter.CharacterId;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

    public override float H => 0.96f;
    public override float S => 0.76f;
    public override float V => 0.84f;

    public override Color DeckEntryCardColor => CrawlerCharacter.Color;
    public override bool IsColorless => false;
}
