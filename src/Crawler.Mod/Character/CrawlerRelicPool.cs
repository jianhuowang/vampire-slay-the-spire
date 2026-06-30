using BaseLib.Abstracts;
using Crawler.Mod.Extensions;
using Crawler.Mod.Relics;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Crawler.Mod.Character;

public sealed class CrawlerRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => CrawlerCharacter.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

    protected override IEnumerable<RelicModel> GenerateAllRelics() =>
    [
        ModelDb.Relic<TurboturnMeter>()
    ];
}
