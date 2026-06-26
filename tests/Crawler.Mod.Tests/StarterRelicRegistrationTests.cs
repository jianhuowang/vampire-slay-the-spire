namespace Crawler.Mod.Tests;

public sealed class StarterRelicRegistrationTests
{
    private static readonly DirectoryInfo RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void ModelRegistrationAdapterInjectsStarterRelic()
    {
        var adapterPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Adapters", "ModelRegistrationAdapter.cs");
        var adapterText = File.ReadAllText(adapterPath);

        Assert.Contains("ModelDb.Inject(typeof(TurboturnMeter));", adapterText);
    }

    [Fact]
    public void CrawlerStarterRelicUsesCrawlerPoolAndPlaceholderImages()
    {
        var relicPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Relics", "CrawlerRelic.cs");
        var relicText = File.ReadAllText(relicPath);

        Assert.Contains("CustomRelicModel", relicText);
        Assert.Contains("[Pool(typeof(CrawlerRelicPool))]", relicText);
        Assert.Contains("\"relic.png\".RelicImagePath()", relicText);
        Assert.Contains("\"relic_outline.png\".RelicImagePath()", relicText);
        Assert.Contains("\"relic.png\".BigRelicImagePath()", relicText);
    }

    [Fact]
    public void TurboturnMeterDefinesStarterRelicModel()
    {
        var relicPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Relics", "TurboturnMeter.cs");
        var relicText = File.ReadAllText(relicPath);

        Assert.Contains("namespace Crawler.Mod.Relics;", relicText);
        Assert.Contains("public sealed class TurboturnMeter() : CrawlerRelic", relicText);
        Assert.Contains("RelicRarity.Starter", relicText);
    }

    [Fact]
    public void TurboturnMeterShowsCurrentChainMultiplierAsRelicCounter()
    {
        var relicPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Relics", "TurboturnMeter.cs");
        var relicText = File.ReadAllText(relicPath);

        Assert.Contains("public override bool ShowCounter => true;", relicText);
        Assert.Contains("public override bool ShouldReceiveCombatHooks => true;", relicText);
        Assert.Contains("public override int DisplayAmount", relicText);
        Assert.Contains("CrawlerChainRuntime.GetCurrentState(Owner).Multiplier", relicText);
        Assert.Contains("Math.Max(1,", relicText);
    }

    [Fact]
    public void TurboturnMeterRefreshesCounterWhenChainCanChange()
    {
        var relicPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Relics", "TurboturnMeter.cs");
        var relicText = File.ReadAllText(relicPath);

        Assert.Contains("AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)", relicText);
        Assert.Contains("AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)", relicText);
        Assert.Contains("InvokeDisplayAmountChanged();", relicText);
        Assert.Contains("Flash();", relicText);
    }

    [Fact]
    public void EnglishLocalizationDefinesTurboturnMeterRules()
    {
        var localizationPath = Path.Combine(
            RepositoryRoot.FullName,
            "src",
            "Crawler.Mod",
            "VampireCrawler",
            "localization",
            "eng",
            "relics.json");
        var localizationText = File.ReadAllText(localizationPath);

        Assert.Contains("CRAWLER-TURBOTURN_METER.title", localizationText);
        Assert.Contains("CRAWLER-TURBOTURN_METER.description", localizationText);
        Assert.Contains("CRAWLER-TURBOTURN_METER.flavor", localizationText);
        Assert.Contains("chain resets to 1x", localizationText);
        Assert.Contains("next cost", localizationText);
        Assert.Contains("multiplier", localizationText);
        Assert.Contains("Auto-played", localizationText);
        Assert.Contains("Crawler card damage, Block, draw, Weak, and Doom", localizationText);
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
}
