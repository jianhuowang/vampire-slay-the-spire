using Crawler.Mod.Adapters;

namespace Crawler.Mod;

public sealed class CrawlerMod
{
    public ChainCombatAdapter ChainCombat { get; } = new();
    public ModelRegistrationAdapter ModelRegistration { get; } = new();

    public void Initialize()
    {
        ModelRegistration.RegisterModels();
    }
}
