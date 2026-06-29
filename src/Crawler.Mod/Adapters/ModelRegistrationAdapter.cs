using Crawler.Mod.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Crawler.Mod.Adapters;

public sealed class ModelRegistrationAdapter
{
    public void RegisterModels()
    {
        ModelDb.Inject(typeof(BloodTap));
        ModelDb.Inject(typeof(SanguineGuard));
        ModelDb.Inject(typeof(MawStrike));
        ModelDb.Inject(typeof(HematicStep));
        ModelDb.Inject(typeof(BleedingLash));
        ModelDb.Inject(typeof(GraveBloom));
    }
}
