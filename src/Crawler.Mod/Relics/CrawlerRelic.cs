using BaseLib.Abstracts;
using BaseLib.Utils;
using Crawler.Mod.Character;
using Crawler.Mod.Extensions;

namespace Crawler.Mod.Relics;

[Pool(typeof(CrawlerRelicPool))]
public abstract class CrawlerRelic : CustomRelicModel
{
    protected CrawlerRelic() : base(false)
    {
    }

    public override string PackedIconPath => "relic.png".RelicImagePath();
    protected override string PackedIconOutlinePath => "relic_outline.png".RelicImagePath();
    protected override string BigIconPath => "relic.png".BigRelicImagePath();
}
