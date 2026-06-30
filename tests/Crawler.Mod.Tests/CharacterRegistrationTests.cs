namespace Crawler.Mod.Tests;

public sealed class CharacterRegistrationTests
{
    private static readonly DirectoryInfo RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void ModEntryPointUsesTemplateInitializationOnly()
    {
        var entryPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "ModEntry", "MainFile.cs");
        var entryText = File.ReadAllText(entryPath);

        Assert.Contains("Harmony harmony = new(ModId);", entryText);
        Assert.Contains("harmony.PatchAll();", entryText);
        Assert.DoesNotContain("new CrawlerMod().Initialize();", entryText);
        Assert.DoesNotContain("RegisterModels", entryText);
    }

    [Fact]
    public void RuntimeModDoesNotUseManualModelInjection()
    {
        var sourceText = ReadRuntimeModSource();

        Assert.DoesNotContain("ModelDb.Inject(", sourceText);
        Assert.DoesNotContain("ModelRegistrationAdapter", sourceText);
    }

    [Fact]
    public void CrawlerCharacterDefinesPlayableStarterShell()
    {
        var characterPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Character", "CrawlerCharacter.cs");
        var characterText = File.ReadAllText(characterPath);

        Assert.Contains("PlaceholderCharacterModel", characterText);
        Assert.Contains("CharacterId = \"VampireCrawler.TheCrawler\"", characterText);
        Assert.Contains("StartingHp => 70", characterText);
        Assert.Contains("ModelDb.Card<QuickStab>()", characterText);
        Assert.Contains("ModelDb.Card<WhipCrack>()", characterText);
        Assert.Contains("ModelDb.Card<GuardedDash>()", characterText);
        Assert.Contains("ModelDb.Card<HeavySwing>()", characterText);
        Assert.Contains("ModelDb.Card<PocketWatch>()", characterText);
        Assert.Contains("ModelDb.Card<BadOmen>()", characterText);
        Assert.Contains("ModelDb.Relic<TurboturnMeter>()", characterText);
        Assert.DoesNotContain("ModelDb.Relic<BurningBlood>()", characterText);
        Assert.Contains("ModelDb.CardPool<CrawlerCardPool>()", characterText);
        Assert.Contains("ModelDb.RelicPool<CrawlerRelicPool>()", characterText);
        Assert.Contains("ModelDb.PotionPool<CrawlerPotionPool>()", characterText);
    }

    [Fact]
    public void CrawlerPoolsDeclareCharacterColorAndAssetPaths()
    {
        var characterDirectory = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Character");
        var cardPoolText = File.ReadAllText(Path.Combine(characterDirectory, "CrawlerCardPool.cs"));
        var relicPoolText = File.ReadAllText(Path.Combine(characterDirectory, "CrawlerRelicPool.cs"));
        var potionPoolText = File.ReadAllText(Path.Combine(characterDirectory, "CrawlerPotionPool.cs"));

        Assert.Contains("CustomCardPoolModel", cardPoolText);
        Assert.Contains("Title => CrawlerCharacter.CharacterId", cardPoolText);
        Assert.Contains("DeckEntryCardColor => CrawlerCharacter.Color", cardPoolText);
        Assert.Contains("IsColorless => false", cardPoolText);

        Assert.Contains("CustomRelicPoolModel", relicPoolText);
        Assert.Contains("LabOutlineColor => CrawlerCharacter.Color", relicPoolText);

        Assert.Contains("CustomPotionPoolModel", potionPoolText);
        Assert.Contains("LabOutlineColor => CrawlerCharacter.Color", potionPoolText);

        Assert.Contains("big_energy.png", cardPoolText);
        Assert.Contains("text_energy.png", cardPoolText);
        Assert.Contains("big_energy.png", relicPoolText);
        Assert.Contains("text_energy.png", relicPoolText);
        Assert.Contains("big_energy.png", potionPoolText);
        Assert.Contains("text_energy.png", potionPoolText);
    }

    [Fact]
    public void EnglishLocalizationDefinesCrawlerCharacterKeysRequiredByAnalyzer()
    {
        var localizationPath = Path.Combine(
            RepositoryRoot.FullName,
            "src",
            "Crawler.Mod",
            "VampireCrawler",
            "localization",
            "eng",
            "characters.json");
        var localizationText = File.ReadAllText(localizationPath);

        string[] requiredKeys =
        [
            "CRAWLER-CRAWLER_CHARACTER.title",
            "CRAWLER-CRAWLER_CHARACTER.titleObject",
            "CRAWLER-CRAWLER_CHARACTER.description",
            "CRAWLER-CRAWLER_CHARACTER.pronounObject",
            "CRAWLER-CRAWLER_CHARACTER.possessiveAdjective",
            "CRAWLER-CRAWLER_CHARACTER.pronounPossessive",
            "CRAWLER-CRAWLER_CHARACTER.pronounSubject",
            "CRAWLER-CRAWLER_CHARACTER.goldMonologue",
            "CRAWLER-CRAWLER_CHARACTER.eventDeathPrevention",
            "CRAWLER-CRAWLER_CHARACTER.aromaPrinciple",
            "CRAWLER-CRAWLER_CHARACTER.cardsModifierTitle",
            "CRAWLER-CRAWLER_CHARACTER.cardsModifierDescription",
            "CRAWLER-CRAWLER_CHARACTER.banter.alive.endTurnPing",
            "CRAWLER-CRAWLER_CHARACTER.banter.dead.endTurnPing"
        ];

        foreach (var key in requiredKeys)
        {
            Assert.Contains(key, localizationText);
        }
    }

    [Fact]
    public void EnglishLocalizationDefinesArchitectDialogueKeysRequiredByAnalyzer()
    {
        var localizationPath = Path.Combine(
            RepositoryRoot.FullName,
            "src",
            "Crawler.Mod",
            "VampireCrawler",
            "localization",
            "eng",
            "ancients.json");
        var localizationText = File.ReadAllText(localizationPath);

        string[] requiredKeys =
        [
            "THE_ARCHITECT.talk.CRAWLER-CRAWLER_CHARACTER.0-0r.char",
            "THE_ARCHITECT.talk.CRAWLER-CRAWLER_CHARACTER.0-0r.next",
            "THE_ARCHITECT.talk.CRAWLER-CRAWLER_CHARACTER.0-1r.ancient",
            "THE_ARCHITECT.talk.CRAWLER-CRAWLER_CHARACTER.0-attack"
        ];

        foreach (var key in requiredKeys)
        {
            Assert.Contains(key, localizationText);
        }
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
