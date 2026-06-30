using Crawler.Content;

namespace Crawler.Content.Tests;

public sealed class CardCatalogTests
{
    [Fact]
    public void V1CatalogHasFullCharacterCardCount()
    {
        var cards = CardCatalog.LoadV1();

        Assert.Equal(66, cards.Count);
    }

    [Fact]
    public void V1CatalogContainsAllThreeRoutes()
    {
        var routes = CardCatalog.LoadV1().GroupBy(card => card.Route).ToDictionary(group => group.Key, group => group.Count());

        Assert.Equal(27, routes["Weapon"]);
        Assert.Equal(23, routes["Item"]);
        Assert.Equal(16, routes["Curse"]);
    }

    [Fact]
    public void V1CatalogHasEnoughCostsForChains()
    {
        var costs = CardCatalog.LoadV1().GroupBy(card => card.Cost).ToDictionary(group => group.Key, group => group.Count());

        Assert.Equal(9, costs[0]);
        Assert.Equal(22, costs[1]);
        Assert.Equal(19, costs[2]);
        Assert.Equal(12, costs[3]);
        Assert.Equal(4, costs[4]);
    }

    [Fact]
    public void V1CatalogContainsStarterFacingCards()
    {
        var cards = CardCatalog.LoadV1().ToDictionary(card => card.Id, StringComparer.Ordinal);

        AssertCard(cards["crawler_quick_stab"], "Quick Stab", 0, "Weapon", "Starter");
        AssertCard(cards["crawler_whip_crack"], "Whip Crack", 1, "Weapon", "Starter");
        AssertCard(cards["crawler_guarded_dash"], "Guarded Dash", 1, "Item", "Starter");
        AssertCard(cards["crawler_heavy_swing"], "Heavy Swing", 2, "Weapon", "Starter");
        AssertCard(cards["crawler_pocket_watch"], "Pocket Watch", 0, "Item", "Starter");
        Assert.Contains("wildcard", cards["crawler_pocket_watch"].Tags);
        AssertCard(cards["crawler_bad_omen"], "Bad Omen", 1, "Curse", "Starter");
    }

    [Fact]
    public void EveryCardHasStableIdentityAndAtLeastOneEffect()
    {
        var cards = CardCatalog.LoadV1();
        var allowedRarities = new HashSet<string>(StringComparer.Ordinal)
        {
            "Starter",
            "Common",
            "Uncommon",
            "Rare",
            "Curse"
        };

        Assert.Equal(cards.Count, cards.Select(card => card.Id).Distinct(StringComparer.Ordinal).Count());
        Assert.All(cards, card =>
        {
            Assert.Matches("^[a-z0-9_]+$", card.Id);
            Assert.False(string.IsNullOrWhiteSpace(card.Name));
            Assert.False(string.IsNullOrWhiteSpace(card.Rarity));
            Assert.Contains(card.Rarity, allowedRarities);
            Assert.NotEmpty(card.Effects);
        });
    }

    private static void AssertCard(Crawler.Core.Cards.CardDefinition card, string name, int cost, string route, string rarity)
    {
        Assert.Equal(name, card.Name);
        Assert.Equal(cost, card.Cost);
        Assert.Equal(route, card.Route);
        Assert.Equal(rarity, card.Rarity);
    }
}
