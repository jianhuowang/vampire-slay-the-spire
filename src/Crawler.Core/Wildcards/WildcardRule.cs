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
