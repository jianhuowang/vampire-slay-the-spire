using Crawler.Mod.ModEntry;

namespace Crawler.Mod.Extensions;

public static class ResourcePathExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", path);
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "charui", path);
    }
}
