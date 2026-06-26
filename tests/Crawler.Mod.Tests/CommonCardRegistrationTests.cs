namespace Crawler.Mod.Tests;

public sealed class CommonCardRegistrationTests
{
    private static readonly DirectoryInfo RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void ModelRegistrationAdapterInjectsCommonCards()
    {
        var adapterPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Adapters", "ModelRegistrationAdapter.cs");
        var adapterText = File.ReadAllText(adapterPath);

        Assert.Contains("ModelDb.Inject(typeof(BloodTap));", adapterText);
        Assert.Contains("ModelDb.Inject(typeof(SanguineGuard));", adapterText);
        Assert.Contains("ModelDb.Inject(typeof(MawStrike));", adapterText);
    }

    [Fact]
    public void CommonCardsDefineExpectedCostsRaritiesAndTargets()
    {
        var cardsPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "CommonCards.cs");
        var cardsText = File.ReadAllText(cardsPath);

        Assert.Contains("BloodTap() : CrawlerCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)", cardsText);
        Assert.Contains("SanguineGuard() : CrawlerCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)", cardsText);
        Assert.Contains("MawStrike() : CrawlerCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)", cardsText);
    }

    [Fact]
    public void CommonCardsImplementChainScaledEffects()
    {
        var cardsPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "CommonCards.cs");
        var cardsText = File.ReadAllText(cardsPath);

        Assert.Equal(3, CountOccurrences(cardsText, "var multiplier = ResolveChainMultiplier(cardPlay);"));
        Assert.Contains("await DrawCards(choiceContext, ApplyMultiplier(UpgradeValue(1, 2), multiplier));", cardsText);
        Assert.Contains("await GainBlock(cardPlay, ApplyMultiplier(UpgradeValue(6, 9), multiplier));", cardsText);
        Assert.Contains("await DealDamage(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(12, 16), multiplier));", cardsText);
        Assert.Contains("await ApplyDoom(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(1, 2), multiplier));", cardsText);
    }

    [Fact]
    public void EnglishLocalizationDefinesCommonCardKeys()
    {
        var localizationPath = Path.Combine(
            RepositoryRoot.FullName,
            "src",
            "Crawler.Mod",
            "VampireCrawler",
            "localization",
            "eng",
            "cards.json");
        var localizationText = File.ReadAllText(localizationPath);

        string[] requiredKeys =
        [
            "CRAWLER-BLOOD_TAP.title",
            "CRAWLER-BLOOD_TAP.description",
            "CRAWLER-SANGUINE_GUARD.title",
            "CRAWLER-SANGUINE_GUARD.description",
            "CRAWLER-MAW_STRIKE.title",
            "CRAWLER-MAW_STRIKE.description"
        ];

        foreach (var key in requiredKeys)
        {
            Assert.Contains(key, localizationText);
        }
    }

    [Fact]
    public void EnglishLocalizationUsesUpgradeSwapMarkersForCommonCards()
    {
        var localizationPath = Path.Combine(
            RepositoryRoot.FullName,
            "src",
            "Crawler.Mod",
            "VampireCrawler",
            "localization",
            "eng",
            "cards.json");
        var localizationText = File.ReadAllText(localizationPath);

        Assert.Contains("Draw -1 card-+2 cards+. Chain multiplier applies.", localizationText);
        Assert.Contains("Gain -6-+9+ Block. Chain multiplier applies.", localizationText);
        Assert.Contains("Deal -12-+16+ damage. Apply -1-+2+ Doom. Chain multiplier applies.", localizationText);
    }

    private static DirectoryInfo FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "Crawler.sln")))
            {
                return current;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root.");
    }

    private static int CountOccurrences(string text, string value)
    {
        var count = 0;
        var index = 0;
        while ((index = text.IndexOf(value, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += value.Length;
        }

        return count;
    }
}
