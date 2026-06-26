using MegaCrit.Sts2.Core.Entities.Cards;

namespace Crawler.Mod.Cards;

public sealed class QuickStab() : CrawlerCard(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy);

public sealed class WhipCrack() : CrawlerCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy);

public sealed class GuardedDash() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self);

public sealed class HeavySwing() : CrawlerCard(2, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy);

public sealed class PocketWatch() : CrawlerCard(0, CardType.Skill, CardRarity.Basic, TargetType.Self);

public sealed class BadOmen() : CrawlerCard(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy);
