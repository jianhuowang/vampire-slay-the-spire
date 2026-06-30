namespace Crawler.Mod.Tests;

public sealed class PlaytestDocumentationTests
{
    private static readonly DirectoryInfo RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void DraftPlaytestChecklistExists()
    {
        var checklistPath = Path.Combine(RepositoryRoot.FullName, "docs", "playtest", "v0-feedback-checklist.md");

        Assert.True(File.Exists(checklistPath), "Expected a v0 playtest checklist for collecting friend feedback.");
    }

    [Fact]
    public void DraftPlaytestChecklistCoversCoreFeedbackAreas()
    {
        var checklistPath = Path.Combine(RepositoryRoot.FullName, "docs", "playtest", "v0-feedback-checklist.md");
        var checklistText = File.ReadAllText(checklistPath);

        Assert.Contains("启动检查", checklistText);
        Assert.Contains("机制反馈", checklistText);
        Assert.Contains("卡牌反馈", checklistText);
        Assert.Contains("崩溃或异常", checklistText);
        Assert.Contains("一句话结论", checklistText);
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
