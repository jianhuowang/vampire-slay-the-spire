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
