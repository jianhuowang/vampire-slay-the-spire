# Vampire Crawlers Character Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build the first playable foundation for a Slay the Spire 2 fan character inspired by Vampire Crawlers, centered on strict rising-cost chain multipliers.

**Architecture:** Keep chain rules and card data in a tested C# core library that does not depend on Slay the Spire 2 internals. Add a thin mod adapter layer later so Early Access API changes only affect integration files, not the gameplay model.

**Tech Stack:** C#/.NET class library, xUnit tests, JSON content catalogs, Godot/.NET mod adapter boundary for Slay the Spire 2.

## Global Constraints

- Full v1 target: complete starter deck, about 60-75 cards, 8-12 character relics, and a clear build identity.
- Core rule: the first card played each turn resolves at `1x` and starts a chain from its played cost.
- Core rule: if the next played card costs exactly `last played cost + 1`, it resolves at `current multiplier + 1`.
- Core rule: if the next played card does not meet that exact cost, it resolves at `1x` and starts a new chain from its played cost.
- Core rule: X-cost cards use the amount of energy actually spent as their played cost.
- Core rule: only cards intentionally played by the player update the chain.
- Multiplier scope: damage, block, healing, card draw, energy gain, buff stacks gained, debuff stacks applied, and generated card counts.
- Build routes: Weapon Chain, Item Chain, and Curse Chain.
- Wildcards bend chain rules but should not replace the core mechanic.
- Out of scope for v1: custom events, custom potions, full original art pass, large narrative integration, perfect public-release asset cleanup.

---

## File Structure

- `Crawler.sln` - solution file containing core, content, mod adapter, and tests.
- `src/Crawler.Core/Crawler.Core.csproj` - pure gameplay model with no game API dependencies.
- `src/Crawler.Core/Chains/CostChainState.cs` - immutable chain state and transition result.
- `src/Crawler.Core/Chains/CostChainEngine.cs` - strict rising-cost chain transition rules.
- `src/Crawler.Core/Cards/NumericEffect.cs` - numeric card effect model.
- `src/Crawler.Core/Cards/CardDefinition.cs` - card definition model used by catalogs and adapters.
- `src/Crawler.Core/Cards/MultiplierApplicator.cs` - multiplies eligible numeric effects.
- `src/Crawler.Core/Wildcards/WildcardRule.cs` - data model for chain-bending effects.
- `src/Crawler.Core/Wildcards/WildcardResolver.cs` - applies Wildcard modifications before chain resolution.
- `src/Crawler.Content/Crawler.Content.csproj` - embedded JSON card and relic catalogs plus validation helpers.
- `src/Crawler.Content/cards.v1.json` - full v1 card catalog.
- `src/Crawler.Content/relics.v1.json` - starting relic and character relic catalog.
- `src/Crawler.Mod/Crawler.Mod.csproj` - Slay the Spire 2 adapter project.
- `src/Crawler.Mod/CrawlerMod.cs` - mod entry point and registration coordinator.
- `src/Crawler.Mod/Adapters/ChainCombatAdapter.cs` - connects player card plays to `CostChainEngine`.
- `src/Crawler.Mod/Adapters/CardRegistrationAdapter.cs` - converts `CardDefinition` records into game cards.
- `src/Crawler.Mod/Assets/README.md` - asset policy and placeholder art naming rules.
- `tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj` - xUnit tests for pure gameplay logic.
- `tests/Crawler.Core.Tests/CostChainEngineTests.cs` - chain transition tests.
- `tests/Crawler.Core.Tests/MultiplierApplicatorTests.cs` - multiplier scope tests.
- `tests/Crawler.Core.Tests/WildcardResolverTests.cs` - Wildcard behavior tests.
- `tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj` - catalog validation tests.
- `tests/Crawler.Content.Tests/CardCatalogTests.cs` - card pool size, route, and cost distribution tests.
- `tests/Crawler.Content.Tests/RelicCatalogTests.cs` - relic count and starter relic tests.

---

### Task 1: Scaffold Solution And Test Projects

**Files:**
- Create: `global.json`
- Create: `Crawler.sln`
- Create: `src/Crawler.Core/Crawler.Core.csproj`
- Create: `src/Crawler.Content/Crawler.Content.csproj`
- Create: `src/Crawler.Mod/Crawler.Mod.csproj`
- Create: `tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj`
- Create: `tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj`

**Interfaces:**
- Produces: buildable solution with projects named `Crawler.Core`, `Crawler.Content`, `Crawler.Mod`, `Crawler.Core.Tests`, and `Crawler.Content.Tests`.

- [ ] **Step 1: Create the solution and projects**

Run:

```powershell
dotnet new globaljson --sdk-version 10.0.100
dotnet new sln -n Crawler
dotnet new classlib -n Crawler.Core -o src/Crawler.Core
dotnet new classlib -n Crawler.Content -o src/Crawler.Content
dotnet new classlib -n Crawler.Mod -o src/Crawler.Mod
dotnet new xunit -n Crawler.Core.Tests -o tests/Crawler.Core.Tests
dotnet new xunit -n Crawler.Content.Tests -o tests/Crawler.Content.Tests
dotnet sln Crawler.sln add src/Crawler.Core/Crawler.Core.csproj src/Crawler.Content/Crawler.Content.csproj src/Crawler.Mod/Crawler.Mod.csproj tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj
dotnet add src/Crawler.Content/Crawler.Content.csproj reference src/Crawler.Core/Crawler.Core.csproj
dotnet add src/Crawler.Mod/Crawler.Mod.csproj reference src/Crawler.Core/Crawler.Core.csproj src/Crawler.Content/Crawler.Content.csproj
dotnet add tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj reference src/Crawler.Core/Crawler.Core.csproj
dotnet add tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj reference src/Crawler.Core/Crawler.Core.csproj src/Crawler.Content/Crawler.Content.csproj
```

Expected: each command exits with code `0`; `dotnet sln list` shows five projects.

- [ ] **Step 2: Remove template classes**

Delete:

```text
src/Crawler.Core/Class1.cs
src/Crawler.Content/Class1.cs
src/Crawler.Mod/Class1.cs
tests/Crawler.Core.Tests/UnitTest1.cs
tests/Crawler.Content.Tests/UnitTest1.cs
```

- [ ] **Step 3: Verify empty scaffold builds**

Run:

```powershell
dotnet test Crawler.sln
```

Expected: build succeeds and test run reports `0 Failed`.

- [ ] **Step 4: Commit**

```powershell
git add global.json Crawler.sln src tests
git commit -m "chore: scaffold crawler mod solution"
```

---

### Task 2: Implement Strict Cost Chain Engine

**Files:**
- Create: `src/Crawler.Core/Chains/CostChainState.cs`
- Create: `src/Crawler.Core/Chains/CostChainEngine.cs`
- Create: `tests/Crawler.Core.Tests/CostChainEngineTests.cs`

**Interfaces:**
- Produces: `CostChainState(int? LastPlayedCost, int Multiplier)`
- Produces: `ChainResolution(int PlayedCost, int AppliedMultiplier, CostChainState NextState, bool ContinuedChain)`
- Produces: `CostChainEngine.StartTurn()`
- Produces: `CostChainEngine.ResolvePlayedCard(CostChainState state, int playedCost)`

- [ ] **Step 1: Write failing tests**

Create `tests/Crawler.Core.Tests/CostChainEngineTests.cs`:

```csharp
using Crawler.Core.Chains;

namespace Crawler.Core.Tests;

public sealed class CostChainEngineTests
{
    [Fact]
    public void FirstCardStartsChainAtOneMultiplier()
    {
        var state = CostChainEngine.StartTurn();

        var result = CostChainEngine.ResolvePlayedCard(state, playedCost: 1);

        Assert.Equal(1, result.AppliedMultiplier);
        Assert.Equal(1, result.NextState.LastPlayedCost);
        Assert.Equal(1, result.NextState.Multiplier);
        Assert.False(result.ContinuedChain);
    }

    [Fact]
    public void ExactNextCostContinuesChainAndIncreasesMultiplier()
    {
        var state = CostChainEngine.ResolvePlayedCard(CostChainEngine.StartTurn(), 0).NextState;

        var result = CostChainEngine.ResolvePlayedCard(state, playedCost: 1);

        Assert.Equal(2, result.AppliedMultiplier);
        Assert.Equal(1, result.NextState.LastPlayedCost);
        Assert.Equal(2, result.NextState.Multiplier);
        Assert.True(result.ContinuedChain);
    }

    [Fact]
    public void NonMatchingCostResolvesAtOneAndRestartsChain()
    {
        var state = CostChainEngine.ResolvePlayedCard(CostChainEngine.StartTurn(), 0).NextState;
        state = CostChainEngine.ResolvePlayedCard(state, 1).NextState;

        var result = CostChainEngine.ResolvePlayedCard(state, playedCost: 1);

        Assert.Equal(1, result.AppliedMultiplier);
        Assert.Equal(1, result.NextState.LastPlayedCost);
        Assert.Equal(1, result.NextState.Multiplier);
        Assert.False(result.ContinuedChain);
    }
}
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```powershell
dotnet test tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj --filter CostChainEngineTests
```

Expected: build fails because namespace `Crawler.Core.Chains` does not exist.

- [ ] **Step 3: Implement chain state and engine**

Create `src/Crawler.Core/Chains/CostChainState.cs`:

```csharp
namespace Crawler.Core.Chains;

public sealed record CostChainState(int? LastPlayedCost, int Multiplier);

public sealed record ChainResolution(
    int PlayedCost,
    int AppliedMultiplier,
    CostChainState NextState,
    bool ContinuedChain);
```

Create `src/Crawler.Core/Chains/CostChainEngine.cs`:

```csharp
namespace Crawler.Core.Chains;

public static class CostChainEngine
{
    public static CostChainState StartTurn() => new(null, 0);

    public static ChainResolution ResolvePlayedCard(CostChainState state, int playedCost)
    {
        if (playedCost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(playedCost), "Played cost cannot be negative.");
        }

        var continues = state.LastPlayedCost.HasValue && playedCost == state.LastPlayedCost.Value + 1;
        var appliedMultiplier = continues ? state.Multiplier + 1 : 1;
        var nextState = new CostChainState(playedCost, appliedMultiplier);

        return new ChainResolution(playedCost, appliedMultiplier, nextState, continues);
    }
}
```

- [ ] **Step 4: Verify tests pass**

Run:

```powershell
dotnet test tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj --filter CostChainEngineTests
```

Expected: `3 Passed`, `0 Failed`.

- [ ] **Step 5: Commit**

```powershell
git add src/Crawler.Core/Chains tests/Crawler.Core.Tests/CostChainEngineTests.cs
git commit -m "feat: add strict cost chain engine"
```

---

### Task 3: Implement Multiplier Application For Numeric Effects

**Files:**
- Create: `src/Crawler.Core/Cards/NumericEffect.cs`
- Create: `src/Crawler.Core/Cards/CardDefinition.cs`
- Create: `src/Crawler.Core/Cards/MultiplierApplicator.cs`
- Create: `tests/Crawler.Core.Tests/MultiplierApplicatorTests.cs`

**Interfaces:**
- Produces: `NumericEffect(string Key, int BaseValue, bool IsMultiplierEligible = true)`
- Produces: `CardDefinition(string Id, string Name, int Cost, string Route, IReadOnlyList<NumericEffect> Effects, IReadOnlyList<string> Tags)`
- Produces: `MultiplierApplicator.Apply(CardDefinition card, int multiplier)`

- [ ] **Step 1: Write failing tests**

Create `tests/Crawler.Core.Tests/MultiplierApplicatorTests.cs`:

```csharp
using Crawler.Core.Cards;

namespace Crawler.Core.Tests;

public sealed class MultiplierApplicatorTests
{
    [Fact]
    public void MultipliesAllEligibleNumericEffects()
    {
        var card = new CardDefinition(
            "test_burst",
            "Burst",
            2,
            "Weapon",
            new[]
            {
                new NumericEffect("damage", 6),
                new NumericEffect("draw", 1),
                new NumericEffect("energy", 1)
            },
            Array.Empty<string>());

        var result = MultiplierApplicator.Apply(card, multiplier: 3);

        Assert.Equal(18, result["damage"]);
        Assert.Equal(3, result["draw"]);
        Assert.Equal(3, result["energy"]);
    }

    [Fact]
    public void LeavesExplicitlyIneligibleEffectsUnchanged()
    {
        var card = new CardDefinition(
            "test_fixed",
            "Fixed",
            1,
            "Item",
            new[] { new NumericEffect("createdCards", 2, IsMultiplierEligible: false) },
            Array.Empty<string>());

        var result = MultiplierApplicator.Apply(card, multiplier: 5);

        Assert.Equal(2, result["createdCards"]);
    }
}
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```powershell
dotnet test tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj --filter MultiplierApplicatorTests
```

Expected: build fails because namespace `Crawler.Core.Cards` does not exist.

- [ ] **Step 3: Implement card models and multiplier**

Create `src/Crawler.Core/Cards/NumericEffect.cs`:

```csharp
namespace Crawler.Core.Cards;

public sealed record NumericEffect(string Key, int BaseValue, bool IsMultiplierEligible = true);
```

Create `src/Crawler.Core/Cards/CardDefinition.cs`:

```csharp
namespace Crawler.Core.Cards;

public sealed record CardDefinition(
    string Id,
    string Name,
    int Cost,
    string Route,
    IReadOnlyList<NumericEffect> Effects,
    IReadOnlyList<string> Tags);
```

Create `src/Crawler.Core/Cards/MultiplierApplicator.cs`:

```csharp
namespace Crawler.Core.Cards;

public static class MultiplierApplicator
{
    public static IReadOnlyDictionary<string, int> Apply(CardDefinition card, int multiplier)
    {
        if (multiplier < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be at least 1.");
        }

        return card.Effects.ToDictionary(
            effect => effect.Key,
            effect => effect.IsMultiplierEligible ? effect.BaseValue * multiplier : effect.BaseValue);
    }
}
```

- [ ] **Step 4: Verify tests pass**

Run:

```powershell
dotnet test tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj --filter MultiplierApplicatorTests
```

Expected: `2 Passed`, `0 Failed`.

- [ ] **Step 5: Commit**

```powershell
git add src/Crawler.Core/Cards tests/Crawler.Core.Tests/MultiplierApplicatorTests.cs
git commit -m "feat: apply chain multipliers to card numbers"
```

---

### Task 4: Add Wildcard Rule Resolution

**Files:**
- Create: `src/Crawler.Core/Wildcards/WildcardRule.cs`
- Create: `src/Crawler.Core/Wildcards/WildcardResolver.cs`
- Create: `tests/Crawler.Core.Tests/WildcardResolverTests.cs`

**Interfaces:**
- Produces: `WildcardKind.TreatAsNextCost`, `WildcardKind.PreserveMultiplierOnMiss`, `WildcardKind.CopyPreviousCost`
- Produces: `WildcardContext(int? LastPlayedCost, int CurrentMultiplier, int PrintedPlayedCost)`
- Produces: `WildcardResolution(int EffectivePlayedCost, bool PreserveMultiplierOnMiss)`
- Produces: `WildcardResolver.Resolve(WildcardContext context, IReadOnlyList<WildcardRule> rules)`

- [ ] **Step 1: Write failing tests**

Create `tests/Crawler.Core.Tests/WildcardResolverTests.cs`:

```csharp
using Crawler.Core.Wildcards;

namespace Crawler.Core.Tests;

public sealed class WildcardResolverTests
{
    [Fact]
    public void TreatAsNextCostUsesPreviousCostPlusOne()
    {
        var context = new WildcardContext(LastPlayedCost: 2, CurrentMultiplier: 3, PrintedPlayedCost: 0);
        var rules = new[] { new WildcardRule(WildcardKind.TreatAsNextCost) };

        var result = WildcardResolver.Resolve(context, rules);

        Assert.Equal(3, result.EffectivePlayedCost);
        Assert.False(result.PreserveMultiplierOnMiss);
    }

    [Fact]
    public void CopyPreviousCostUsesPreviousCostWhenAvailable()
    {
        var context = new WildcardContext(LastPlayedCost: 1, CurrentMultiplier: 2, PrintedPlayedCost: 3);
        var rules = new[] { new WildcardRule(WildcardKind.CopyPreviousCost) };

        var result = WildcardResolver.Resolve(context, rules);

        Assert.Equal(1, result.EffectivePlayedCost);
    }

    [Fact]
    public void PreserveMultiplierFlagCanBeCarriedToAdapter()
    {
        var context = new WildcardContext(LastPlayedCost: 1, CurrentMultiplier: 2, PrintedPlayedCost: 3);
        var rules = new[] { new WildcardRule(WildcardKind.PreserveMultiplierOnMiss) };

        var result = WildcardResolver.Resolve(context, rules);

        Assert.Equal(3, result.EffectivePlayedCost);
        Assert.True(result.PreserveMultiplierOnMiss);
    }
}
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```powershell
dotnet test tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj --filter WildcardResolverTests
```

Expected: build fails because namespace `Crawler.Core.Wildcards` does not exist.

- [ ] **Step 3: Implement Wildcard models and resolver**

Create `src/Crawler.Core/Wildcards/WildcardRule.cs`:

```csharp
namespace Crawler.Core.Wildcards;

public enum WildcardKind
{
    TreatAsNextCost,
    PreserveMultiplierOnMiss,
    CopyPreviousCost
}

public sealed record WildcardRule(WildcardKind Kind);

public sealed record WildcardContext(int? LastPlayedCost, int CurrentMultiplier, int PrintedPlayedCost);

public sealed record WildcardResolution(int EffectivePlayedCost, bool PreserveMultiplierOnMiss);
```

Create `src/Crawler.Core/Wildcards/WildcardResolver.cs`:

```csharp
namespace Crawler.Core.Wildcards;

public static class WildcardResolver
{
    public static WildcardResolution Resolve(WildcardContext context, IReadOnlyList<WildcardRule> rules)
    {
        var effectiveCost = context.PrintedPlayedCost;
        var preserveMultiplierOnMiss = false;

        foreach (var rule in rules)
        {
            switch (rule.Kind)
            {
                case WildcardKind.TreatAsNextCost:
                    if (context.LastPlayedCost.HasValue)
                    {
                        effectiveCost = context.LastPlayedCost.Value + 1;
                    }
                    break;
                case WildcardKind.CopyPreviousCost:
                    if (context.LastPlayedCost.HasValue)
                    {
                        effectiveCost = context.LastPlayedCost.Value;
                    }
                    break;
                case WildcardKind.PreserveMultiplierOnMiss:
                    preserveMultiplierOnMiss = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rules), $"Unsupported Wildcard kind: {rule.Kind}");
            }
        }

        return new WildcardResolution(effectiveCost, preserveMultiplierOnMiss);
    }
}
```

- [ ] **Step 4: Verify tests pass**

Run:

```powershell
dotnet test tests/Crawler.Core.Tests/Crawler.Core.Tests.csproj --filter WildcardResolverTests
```

Expected: `3 Passed`, `0 Failed`.

- [ ] **Step 5: Commit**

```powershell
git add src/Crawler.Core/Wildcards tests/Crawler.Core.Tests/WildcardResolverTests.cs
git commit -m "feat: add wildcard chain rule resolution"
```

---

### Task 5: Add Card Catalog And Validation Tests

**Files:**
- Create: `src/Crawler.Content/cards.v1.json`
- Create: `src/Crawler.Content/CardCatalog.cs`
- Modify: `src/Crawler.Content/Crawler.Content.csproj`
- Create: `tests/Crawler.Content.Tests/CardCatalogTests.cs`

**Interfaces:**
- Produces: `CardCatalog.LoadV1()`
- Produces: JSON card records with fields `id`, `name`, `cost`, `route`, `rarity`, `effects`, and `tags`.

- [ ] **Step 1: Write failing catalog tests**

Create `tests/Crawler.Content.Tests/CardCatalogTests.cs`:

```csharp
using Crawler.Content;

namespace Crawler.Content.Tests;

public sealed class CardCatalogTests
{
    [Fact]
    public void V1CatalogHasFullCharacterCardCount()
    {
        var cards = CardCatalog.LoadV1();

        Assert.InRange(cards.Count, 60, 75);
    }

    [Fact]
    public void V1CatalogContainsAllThreeRoutes()
    {
        var routes = CardCatalog.LoadV1().GroupBy(card => card.Route).ToDictionary(group => group.Key, group => group.Count());

        Assert.InRange(routes["Weapon"], 25, 30);
        Assert.InRange(routes["Item"], 20, 25);
        Assert.InRange(routes["Curse"], 15, 20);
    }

    [Fact]
    public void V1CatalogHasEnoughCostsForChains()
    {
        var costs = CardCatalog.LoadV1().GroupBy(card => card.Cost).ToDictionary(group => group.Key, group => group.Count());

        Assert.True(costs[0] >= 8);
        Assert.True(costs[1] >= 18);
        Assert.True(costs[2] >= 16);
        Assert.True(costs[3] >= 8);
    }

    [Fact]
    public void EveryCardHasStableIdentityAndAtLeastOneEffect()
    {
        var cards = CardCatalog.LoadV1();

        Assert.Equal(cards.Count, cards.Select(card => card.Id).Distinct(StringComparer.Ordinal).Count());
        Assert.All(cards, card =>
        {
            Assert.Matches("^[a-z0-9_]+$", card.Id);
            Assert.NotWhiteSpace(card.Name);
            Assert.NotEmpty(card.Effects);
        });
    }
}
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```powershell
dotnet test tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj --filter CardCatalogTests
```

Expected: build fails because `CardCatalog` does not exist.

- [ ] **Step 3: Embed catalog JSON**

Modify `src/Crawler.Content/Crawler.Content.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <ProjectReference Include="..\Crawler.Core\Crawler.Core.csproj" />
  </ItemGroup>
  <ItemGroup>
    <EmbeddedResource Include="cards.v1.json" />
  </ItemGroup>
</Project>
```

Create `src/Crawler.Content/cards.v1.json` with 66 records. Use this exact route and cost distribution:

```text
Weapon: 27 cards
Item: 23 cards
Curse: 16 cards
Cost 0: 9 cards
Cost 1: 22 cards
Cost 2: 19 cards
Cost 3: 12 cards
Cost 4: 4 cards
```

Name the first starter-facing cards:

```text
crawler_quick_stab, Quick Stab, cost 0, Weapon
crawler_whip_crack, Whip Crack, cost 1, Weapon
crawler_guarded_dash, Guarded Dash, cost 1, Item
crawler_heavy_swing, Heavy Swing, cost 2, Weapon
crawler_pocket_watch, Pocket Watch, cost 0, Item, tag wildcard
crawler_bad_omen, Bad Omen, cost 1, Curse
```

- [ ] **Step 4: Implement `CardCatalog.LoadV1()`**

Create `src/Crawler.Content/CardCatalog.cs`:

```csharp
using System.Reflection;
using System.Text.Json;
using Crawler.Core.Cards;

namespace Crawler.Content;

public static class CardCatalog
{
    public static IReadOnlyList<CardDefinition> LoadV1()
    {
        var assembly = typeof(CardCatalog).Assembly;
        const string resourceName = "Crawler.Content.cards.v1.json";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        var records = JsonSerializer.Deserialize<List<CardRecord>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Card catalog could not be deserialized.");

        return records.Select(record => new CardDefinition(
            record.Id,
            record.Name,
            record.Cost,
            record.Route,
            record.Effects.Select(effect => new NumericEffect(effect.Key, effect.BaseValue, effect.IsMultiplierEligible)).ToArray(),
            record.Tags)).ToArray();
    }

    private sealed record CardRecord(
        string Id,
        string Name,
        int Cost,
        string Route,
        string Rarity,
        IReadOnlyList<EffectRecord> Effects,
        IReadOnlyList<string> Tags);

    private sealed record EffectRecord(string Key, int BaseValue, bool IsMultiplierEligible = true);
}
```

- [ ] **Step 5: Verify tests pass**

Run:

```powershell
dotnet test tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj --filter CardCatalogTests
```

Expected: `4 Passed`, `0 Failed`.

- [ ] **Step 6: Commit**

```powershell
git add src/Crawler.Content tests/Crawler.Content.Tests/CardCatalogTests.cs
git commit -m "feat: add v1 crawler card catalog"
```

---

### Task 6: Add Relic Catalog And Validation Tests

**Files:**
- Create: `src/Crawler.Content/relics.v1.json`
- Create: `src/Crawler.Content/RelicCatalog.cs`
- Modify: `src/Crawler.Content/Crawler.Content.csproj`
- Create: `tests/Crawler.Content.Tests/RelicCatalogTests.cs`

**Interfaces:**
- Produces: `RelicDefinition(string Id, string Name, string Rarity, string Description, IReadOnlyList<string> Tags)`
- Produces: `RelicCatalog.LoadV1()`

- [ ] **Step 1: Write failing relic tests**

Create `tests/Crawler.Content.Tests/RelicCatalogTests.cs`:

```csharp
using Crawler.Content;

namespace Crawler.Content.Tests;

public sealed class RelicCatalogTests
{
    [Fact]
    public void V1CatalogHasStartingRelicAndCharacterRelics()
    {
        var relics = RelicCatalog.LoadV1();

        Assert.Single(relics.Where(relic => relic.Rarity == "Starter"));
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
```

- [ ] **Step 2: Run tests to verify failure**

Run:

```powershell
dotnet test tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj --filter RelicCatalogTests
```

Expected: build fails because `RelicCatalog` does not exist.

- [ ] **Step 3: Add relic model and catalog**

Create `src/Crawler.Content/RelicCatalog.cs`:

```csharp
using System.Reflection;
using System.Text.Json;

namespace Crawler.Content;

public sealed record RelicDefinition(
    string Id,
    string Name,
    string Rarity,
    string Description,
    IReadOnlyList<string> Tags);

public static class RelicCatalog
{
    public static IReadOnlyList<RelicDefinition> LoadV1()
    {
        var assembly = typeof(RelicCatalog).Assembly;
        const string resourceName = "Crawler.Content.relics.v1.json";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource not found: {resourceName}");

        return JsonSerializer.Deserialize<List<RelicDefinition>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Relic catalog could not be deserialized.");
    }
}
```

Modify `src/Crawler.Content/Crawler.Content.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <ProjectReference Include="..\Crawler.Core\Crawler.Core.csproj" />
  </ItemGroup>
  <ItemGroup>
    <EmbeddedResource Include="cards.v1.json" />
    <EmbeddedResource Include="relics.v1.json" />
  </ItemGroup>
</Project>
```

Create `src/Crawler.Content/relics.v1.json` with 10 records: 1 `Starter`, 3 `Common`, 3 `Uncommon`, 2 `Rare`, and 1 `Boss`. Include this starter relic as the first record:

```json
{
  "id": "crawler_turboturn_meter",
  "name": "Turboturn Meter",
  "rarity": "Starter",
  "description": "Shows your current chain multiplier and next cost. The first time each combat you resolve a card at 3x or higher, gain 1 temporary energy.",
  "tags": ["chain", "energy"]
}
```

- [ ] **Step 4: Verify tests pass**

Run:

```powershell
dotnet test tests/Crawler.Content.Tests/Crawler.Content.Tests.csproj --filter RelicCatalogTests
```

Expected: `3 Passed`, `0 Failed`.

- [ ] **Step 5: Commit**

```powershell
git add src/Crawler.Content tests/Crawler.Content.Tests/RelicCatalogTests.cs
git commit -m "feat: add v1 crawler relic catalog"
```

---

### Task 7: Add Mod Adapter Boundary

**Files:**
- Create: `src/Crawler.Mod/CrawlerMod.cs`
- Create: `src/Crawler.Mod/Adapters/ChainCombatAdapter.cs`
- Create: `src/Crawler.Mod/Adapters/CardRegistrationAdapter.cs`
- Create: `src/Crawler.Mod/Assets/README.md`

**Interfaces:**
- Produces: `CrawlerMod.Initialize()`
- Produces: `ChainCombatAdapter.StartTurn()`
- Produces: `ChainCombatAdapter.ResolvePlayerPlayedCard(int actualPlayedCost)`
- Produces: `CardRegistrationAdapter.GetCardDefinitionsForRegistration()`

- [ ] **Step 1: Create adapter entry point**

Create `src/Crawler.Mod/CrawlerMod.cs`:

```csharp
using Crawler.Mod.Adapters;

namespace Crawler.Mod;

public sealed class CrawlerMod
{
    public ChainCombatAdapter ChainCombat { get; } = new();
    public CardRegistrationAdapter CardRegistration { get; } = new();

    public void Initialize()
    {
        _ = CardRegistration.GetCardDefinitionsForRegistration();
    }
}
```

- [ ] **Step 2: Create chain combat adapter**

Create `src/Crawler.Mod/Adapters/ChainCombatAdapter.cs`:

```csharp
using Crawler.Core.Chains;

namespace Crawler.Mod.Adapters;

public sealed class ChainCombatAdapter
{
    private CostChainState _state = CostChainEngine.StartTurn();

    public CostChainState CurrentState => _state;

    public void StartTurn()
    {
        _state = CostChainEngine.StartTurn();
    }

    public ChainResolution ResolvePlayerPlayedCard(int actualPlayedCost)
    {
        var result = CostChainEngine.ResolvePlayedCard(_state, actualPlayedCost);
        _state = result.NextState;
        return result;
    }
}
```

- [ ] **Step 3: Create card registration adapter**

Create `src/Crawler.Mod/Adapters/CardRegistrationAdapter.cs`:

```csharp
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
```

- [ ] **Step 4: Document temporary asset rules**

Create `src/Crawler.Mod/Assets/README.md`:

```markdown
# Asset Notes

This folder is for placeholder and reference-driven assets used by the Vampire Crawlers fan mod.

Rules for v1:

- Use simple placeholder images while gameplay is being tested.
- Prefix all files with `crawler_`.
- Keep source notes beside direct reference assets.
- Add a visible non-official fan-mod disclaimer before broad public release.
```

- [ ] **Step 5: Verify adapter builds**

Run:

```powershell
dotnet build src/Crawler.Mod/Crawler.Mod.csproj
```

Expected: build succeeds with `0 Error(s)`.

- [ ] **Step 6: Commit**

```powershell
git add src/Crawler.Mod
git commit -m "feat: add crawler mod adapter boundary"
```

---

### Task 8: Run Full Verification And Prepare Implementation Notes

**Files:**
- Modify: `docs/superpowers/plans/2026-06-25-vampire-crawlers-character-implementation.md`

**Interfaces:**
- Consumes: all tasks above.
- Produces: verified baseline ready for Slay the Spire 2 API-specific registration work.

- [x] **Step 1: Run full test suite**

Run:

```powershell
dotnet test Crawler.sln
```

Expected: all tests pass with `0 Failed`.

- [x] **Step 2: Run full build**

Run:

```powershell
dotnet build Crawler.sln
```

Expected: build succeeds with `0 Error(s)`.

- [x] **Step 3: Inspect git status**

Run:

```powershell
git status --short
```

Expected: no output.

- [x] **Step 4: Record next integration target**

Add this note under the final completed task in the implementation log or PR description:

```markdown
Next integration target: connect `Crawler.Mod.CrawlerMod.Initialize()` to the current Slay the Spire 2 mod loader entry point, then map `CardDefinition` records into the game's runtime card definitions.
```

- [x] **Step 5: Commit any verification note changes**

```powershell
git add docs/superpowers/plans/2026-06-25-vampire-crawlers-character-implementation.md
git commit -m "docs: record crawler implementation verification path"
```

Next integration target: connect `Crawler.Mod.CrawlerMod.Initialize()` to the current Slay the Spire 2 mod loader entry point, then map `CardDefinition` records into the game's runtime card definitions.

---

## Self-Review

- Spec coverage: the plan covers core chain rules, multiplier scope, Wildcards, starter relic direction, card pool size, route distribution, relic target count, and asset-disclaimer policy.
- Intentional gap: direct Slay the Spire 2 runtime API registration is isolated behind `src/Crawler.Mod/Adapters` because Early Access APIs can change. The first implementation must produce a tested adapter boundary before binding to concrete game APIs.
- Forbidden-marker scan: passed, and no undefined function names are used in task interfaces.
- Type consistency: `CostChainState`, `ChainResolution`, `CardDefinition`, `NumericEffect`, `WildcardRule`, `RelicDefinition`, and adapter method names match across tasks.
