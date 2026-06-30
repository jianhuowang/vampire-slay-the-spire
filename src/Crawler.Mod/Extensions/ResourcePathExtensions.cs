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

    public static string CardImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "card_portraits", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "card_portraits", "big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(MainFile.ResPath, "images", "relics", "big", path);
    }
}
