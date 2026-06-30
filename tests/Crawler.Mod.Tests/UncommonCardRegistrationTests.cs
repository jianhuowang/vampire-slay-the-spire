namespace Crawler.Mod.Tests;

public sealed class UncommonCardRegistrationTests
{
    private static readonly DirectoryInfo RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void RuntimeModDoesNotInjectUncommonCardsDirectly()
    {
        var sourceText = ReadRuntimeModSource();

        Assert.DoesNotContain("ModelDb.Inject(typeof(HematicStep));", sourceText);
        Assert.DoesNotContain("ModelDb.Inject(typeof(BleedingLash));", sourceText);
        Assert.DoesNotContain("ModelDb.Inject(typeof(GraveBloom));", sourceText);
    }

    [Fact]
    public void UncommonCardsDefineExpectedCostsRaritiesAndTargets()
    {
        var cardsPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "UncommonCards.cs");
        var cardsText = File.ReadAllText(cardsPath);

        Assert.Equal(3, CountOccurrences(cardsText, "[Pool(typeof(CrawlerCardPool))]"));
        Assert.Contains("HematicStep() : CrawlerCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)", cardsText);
        Assert.Contains("BleedingLash() : CrawlerCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)", cardsText);
        Assert.Contains("GraveBloom() : CrawlerCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)", cardsText);
    }

    [Fact]
    public void UncommonCardsImplementChainScaledEffects()
    {
        var cardsPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "UncommonCards.cs");
        var cardsText = File.ReadAllText(cardsPath);

        Assert.Equal(3, CountOccurrences(cardsText, "var multiplier = ResolveChainMultiplier(cardPlay);"));
        Assert.Contains("await GainBlock(cardPlay, ApplyMultiplier(UpgradeValue(4, 6), multiplier));", cardsText);
        Assert.Contains("await DealDamage(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(9, 13), multiplier));", cardsText);
        Assert.Contains("await ApplyWeak(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(1, 2), multiplier));", cardsText);
        Assert.Contains("await ApplyDoom(choiceContext, cardPlay, ApplyMultiplier(UpgradeValue(2, 3), multiplier));", cardsText);
        Assert.Contains("await DrawCards(choiceContext, ApplyMultiplier(UpgradeValue(1, 2), multiplier));", cardsText);
    }

    [Fact]
    public void EnglishLocalizationDefinesUncommonCardKeys()
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
            "CRAWLER-HEMATIC_STEP.title",
            "CRAWLER-HEMATIC_STEP.description",
            "CRAWLER-BLEEDING_LASH.title",
            "CRAWLER-BLEEDING_LASH.description",
            "CRAWLER-GRAVE_BLOOM.title",
            "CRAWLER-GRAVE_BLOOM.description"
        ];

        foreach (var key in requiredKeys)
        {
            Assert.Contains(key, localizationText);
        }
    }

    [Fact]
    public void EnglishLocalizationUsesUpgradeSwapMarkersForUncommonCards()
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

        Assert.Contains("Gain -4-+6+ Block. Chain multiplier applies.", localizationText);
        Assert.Contains("Deal -9-+13+ damage. Apply -1-+2+ Weak. Chain multiplier applies.", localizationText);
        Assert.Contains("Apply -2-+3+ Doom. Draw -1 card-+2 cards+. Chain multiplier applies.", localizationText);
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

    private static string ReadRuntimeModSource()
    {
        var sourceRoot = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod");
        var sourceFiles = Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}.godot{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Order(StringComparer.Ordinal);

        return string.Join(Environment.NewLine, sourceFiles.Select(File.ReadAllText));
    }
}
