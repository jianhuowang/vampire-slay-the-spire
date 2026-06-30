using Crawler.Content;

namespace Crawler.Content.Tests;

public sealed class RelicCatalogTests
{
    [Fact]
    public void V1CatalogHasStartingRelicAndCharacterRelics()
    {
        var relics = RelicCatalog.LoadV1();
        var rarities = relics.GroupBy(relic => relic.Rarity).ToDictionary(group => group.Key, group => group.Count());

        Assert.Equal(10, relics.Count);
        Assert.Single(relics, relic => relic.Rarity == "Starter");
        Assert.Equal(3, rarities["Common"]);
        Assert.Equal(3, rarities["Uncommon"]);
        Assert.Equal(2, rarities["Rare"]);
        Assert.Equal(1, rarities["Boss"]);
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
