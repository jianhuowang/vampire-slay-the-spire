using System.Text.Json;

namespace Crawler.Mod.Tests;

public sealed class ModPackagingTests
{
    private static readonly DirectoryInfo RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void ManifestDeclaresVampireCrawlerMod()
    {
        var manifestPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "VampireCrawler.json");

        using var document = JsonDocument.Parse(File.ReadAllText(manifestPath));
        var root = document.RootElement;

        Assert.Equal("VampireCrawler", root.GetProperty("id").GetString());
        Assert.Equal("Vampire Crawlers: The Crawler", root.GetProperty("name").GetString());
        Assert.True(root.GetProperty("has_dll").GetBoolean());
        Assert.True(root.GetProperty("has_pck").GetBoolean());
        Assert.True(root.GetProperty("affects_gameplay").GetBoolean());

        var baseLibDependency = root.GetProperty("dependencies")
            .EnumerateArray()
            .Single(dependency => dependency.GetProperty("id").GetString() == "BaseLib");
        Assert.False(string.IsNullOrWhiteSpace(baseLibDependency.GetProperty("min_version").GetString()));
    }

    [Fact]
    public void GodotProjectUsesVampireCrawlerAssembly()
    {
        var projectPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "project.godot");
        var projectText = File.ReadAllText(projectPath);

        Assert.Contains("config/name=\"VampireCrawler\"", projectText);
        Assert.Contains("config/icon=\"res://VampireCrawler/mod_image.png\"", projectText);
        Assert.Contains("project/assembly_name=\"VampireCrawler\"", projectText);
    }

    [Fact]
    public void ProjectProvidesLocalConfigurationExample()
    {
        var examplePath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "local.props.example");
        var exampleText = File.ReadAllText(examplePath);

        Assert.Contains("<Sts2Path>", exampleText);
        Assert.Contains("<GodotPath>", exampleText);
    }

    [Fact]
    public void ModEntryPointUsesSts2ModInitializer()
    {
        var entryPath = Path.Combine(RepositoryRoot.FullName, "src", "Crawler.Mod", "ModEntry", "MainFile.cs");
        var entryText = File.ReadAllText(entryPath);

        Assert.Contains("MegaCrit.Sts2.Core.Modding", entryText);
        Assert.Contains("[ModInitializer(nameof(Initialize))]", entryText);
        Assert.Contains("new CrawlerMod().Initialize();", entryText);
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
