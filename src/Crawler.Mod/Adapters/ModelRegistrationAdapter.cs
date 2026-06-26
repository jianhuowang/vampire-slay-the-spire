using Crawler.Mod.Character;
using Crawler.Mod.Cards;
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
        ModelDb.Inject(typeof(QuickStab));
        ModelDb.Inject(typeof(WhipCrack));
        ModelDb.Inject(typeof(GuardedDash));
        ModelDb.Inject(typeof(HeavySwing));
        ModelDb.Inject(typeof(PocketWatch));
        ModelDb.Inject(typeof(BadOmen));
    }
}
