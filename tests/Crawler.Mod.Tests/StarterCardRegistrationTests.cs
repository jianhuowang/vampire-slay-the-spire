namespace Crawler.Mod.Tests;

public sealed class StarterCardRegistrationTests
{
    private static readonly DirectoryInfo RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void ModelRegistrationAdapterInjectsStarterCards()
    {
        var adapterPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Adapters", "ModelRegistrationAdapter.cs");
        var adapterText = File.ReadAllText(adapterPath);

        Assert.Contains("ModelDb.Inject(typeof(QuickStab));", adapterText);
        Assert.Contains("ModelDb.Inject(typeof(WhipCrack));", adapterText);
        Assert.Contains("ModelDb.Inject(typeof(GuardedDash));", adapterText);
        Assert.Contains("ModelDb.Inject(typeof(HeavySwing));", adapterText);
        Assert.Contains("ModelDb.Inject(typeof(PocketWatch));", adapterText);
        Assert.Contains("ModelDb.Inject(typeof(BadOmen));", adapterText);
    }

    [Fact]
    public void CrawlerCharacterStartsWithCrawlerCards()
    {
        var characterPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Character", "CrawlerCharacter.cs");
        var characterText = File.ReadAllText(characterPath);

        Assert.DoesNotContain("StrikeIronclad", characterText);
        Assert.DoesNotContain("DefendIronclad", characterText);
        Assert.Contains("ModelDb.Card<QuickStab>()", characterText);
        Assert.Contains("ModelDb.Card<WhipCrack>()", characterText);
        Assert.Contains("ModelDb.Card<GuardedDash>()", characterText);
        Assert.Contains("ModelDb.Card<HeavySwing>()", characterText);
        Assert.Contains("ModelDb.Card<PocketWatch>()", characterText);
        Assert.Contains("ModelDb.Card<BadOmen>()", characterText);
    }

    [Fact]
    public void CrawlerCardBaseUsesCrawlerPoolAndPlaceholderPortraits()
    {
        var cardPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "CrawlerCard.cs");
        var cardText = File.ReadAllText(cardPath);

        Assert.Contains("[Pool(typeof(CrawlerCardPool))]", cardText);
        Assert.Contains("CustomCardModel", cardText);
        Assert.Contains("\"card.png\".CardImagePath()", cardText);
        Assert.Contains("\"card.png\".BigCardImagePath()", cardText);
    }

    [Fact]
    public void StarterCardsDefineExpectedCostsAndTargets()
    {
        var cardsPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "StarterCards.cs");
        var cardsText = File.ReadAllText(cardsPath);

        Assert.Contains("QuickStab() : CrawlerCard(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)", cardsText);
        Assert.Contains("WhipCrack() : CrawlerCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)", cardsText);
        Assert.Contains("GuardedDash() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self)", cardsText);
        Assert.Contains("HeavySwing() : CrawlerCard(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)", cardsText);
        Assert.Contains("PocketWatch() : CrawlerCard(0, CardType.Skill, CardRarity.Basic, TargetType.Self)", cardsText);
        Assert.Contains("BadOmen() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy)", cardsText);
    }

    [Fact]
    public void StarterCardsImplementTheirPrintedEffects()
    {
        var cardsPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "StarterCards.cs");
        var cardsText = File.ReadAllText(cardsPath);

        Assert.Equal(6, CountOccurrences(cardsText, "var multiplier = ResolveChainMultiplier(cardPlay);"));
        Assert.Contains("await DealDamage(choiceContext, cardPlay, ApplyMultiplier(5, multiplier));", cardsText);
        Assert.Contains("await DealDamage(choiceContext, cardPlay, ApplyMultiplier(8, multiplier));", cardsText);
        Assert.Contains("await GainBlock(cardPlay, ApplyMultiplier(7, multiplier));", cardsText);
        Assert.Contains("await DealDamage(choiceContext, cardPlay, ApplyMultiplier(14, multiplier));", cardsText);
        Assert.Contains("await DrawCards(choiceContext, ApplyMultiplier(1, multiplier));", cardsText);
        Assert.Contains("await ApplyWeak(choiceContext, cardPlay, ApplyMultiplier(1, multiplier));", cardsText);
        Assert.Contains("await ApplyDoom(choiceContext, cardPlay, ApplyMultiplier(1, multiplier));", cardsText);
    }

    [Fact]
    public void CrawlerCardBaseProvidesEffectCommandHelpers()
    {
        var cardPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Cards", "CrawlerCard.cs");
        var cardText = File.ReadAllText(cardPath);

        Assert.Contains("CreatureCmd.Damage", cardText);
        Assert.Contains("CreatureCmd.GainBlock", cardText);
        Assert.Contains("CardPileCmd.Draw", cardText);
        Assert.Contains("PowerCmd.Apply<WeakPower>", cardText);
        Assert.Contains("PowerCmd.Apply<DoomPower>", cardText);
        Assert.Contains("RequireTarget(cardPlay)", cardText);
        Assert.Contains("if (cardPlay.IsAutoPlay)", cardText);
        Assert.Contains("CrawlerChainRuntime.ResolvePlayedCard(Owner, PrintedCost)", cardText);
        Assert.Contains("ApplyMultiplier(decimal value, int multiplier)", cardText);
    }

    [Fact]
    public void ChainRuntimeTracksStatePerPlayerAndCanReset()
    {
        var runtimePath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Adapters", "CrawlerChainRuntime.cs");
        var runtimeText = File.ReadAllText(runtimePath);

        Assert.Contains("ConcurrentDictionary<ulong, ChainCombatAdapter>", runtimeText);
        Assert.Contains("ResolvePlayedCard(Player player, int playedCost)", runtimeText);
        Assert.Contains("ResetTurn(Player player)", runtimeText);
        Assert.Contains("player.NetId", runtimeText);
    }

    [Fact]
    public void CrawlerCharacterResetsChainAtPlayerTurnStart()
    {
        var characterPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "Character", "CrawlerCharacter.cs");
        var characterText = File.ReadAllText(characterPath);

        Assert.Contains("AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)", characterText);
        Assert.Contains("CrawlerChainRuntime.ResetTurn(player);", characterText);
    }

    [Fact]
    public void EnglishLocalizationDefinesStarterCardKeys()
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
            "CRAWLER-QUICK_STAB.title",
            "CRAWLER-QUICK_STAB.description",
            "CRAWLER-WHIP_CRACK.title",
            "CRAWLER-WHIP_CRACK.description",
            "CRAWLER-GUARDED_DASH.title",
            "CRAWLER-GUARDED_DASH.description",
            "CRAWLER-HEAVY_SWING.title",
            "CRAWLER-HEAVY_SWING.description",
            "CRAWLER-POCKET_WATCH.title",
            "CRAWLER-POCKET_WATCH.description",
            "CRAWLER-BAD_OMEN.title",
            "CRAWLER-BAD_OMEN.description"
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
