using Crawler.Content;
using Crawler.Core.Cards;

namespace Crawler.Mod.Adapters;

public sealed class CardRegistrationAdapter
{
    public IReadOnlyList<CardDefinition> GetCardDefinitionsForRegistration()
    {
        return CardCatalog.LoadV1();
    }
}
