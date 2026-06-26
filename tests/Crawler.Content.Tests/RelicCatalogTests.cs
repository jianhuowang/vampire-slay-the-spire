using Crawler.Content;

namespace Crawler.Content.Tests;

public sealed class RelicCatalogTests
{
    [Fact]
    public void V1CatalogHasStartingRelicAndCharacterRelics()
    {
        var relics = RelicCatalog.LoadV1();

        Assert.Single(relics, relic => relic.Rarity == "Starter");
        Assert.InRange(relics.Count(relic => relic.Rarity != "Starter"), 8, 12);
    }

    [Fact]
    public void StarterRelicShowsChainInformation()
    {
        var starter = RelicCatalog.LoadV1().Single(relic => relic.Rarity == "Starter");

        Assert.Equal("crawler_turboturn_meter", starter.Id);
        Assert.Contains("multiplier", starter.Description, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("next cost", starter.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RelicIdsAreStableAndUnique()
    {
        var relics = RelicCatalog.LoadV1();

        Assert.Equal(relics.Count, relics.Select(relic => relic.Id).Distinct(StringComparer.Ordinal).Count());
        Assert.All(relics, relic => Assert.Matches("^[a-z0-9_]+$", relic.Id));
    }
}
