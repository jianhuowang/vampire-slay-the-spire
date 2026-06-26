using Crawler.Mod.Character;
using MegaCrit.Sts2.Core.Models;

namespace Crawler.Mod.Adapters;

public sealed class ModelRegistrationAdapter
{
    public void RegisterModels()
    {
        ModelDb.Inject(typeof(CrawlerCharacter));
        ModelDb.Inject(typeof(CrawlerCardPool));
        ModelDb.Inject(typeof(CrawlerRelicPool));
        ModelDb.Inject(typeof(CrawlerPotionPool));
    }
}
