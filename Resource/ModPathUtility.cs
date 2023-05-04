using System.Collections.Generic;
using System.IO;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    public static class ModPathUtility
    {
        public const string PackageId = "nomadicooer.rimworld.prf";
        private static readonly string? rootDir;
        private static readonly string? saveDir;
        private readonly static string? filterSettingsPath;
        static ModPathUtility()
        {
            List<ModContentPack> modPacks = LoadedModManager.RunningModsListForReading;
            foreach (var pack in modPacks)
            {
                if (pack.PackageId == PackageId)
                {
                    rootDir = pack.RootDir;
                    saveDir = Path.Combine(rootDir, "Save");
                    if (!Directory.Exists(saveDir))
                    {
                        Directory.CreateDirectory(saveDir);
                    }
                    filterSettingsPath = GetModeDataSavePath("filterSettings.xml");
                    break;
                }
            }
        }
        public static string GetModeDataSavePath(string path) => Path.Combine(saveDir ?? string.Empty, path);
        public static string RootDir => rootDir ?? string.Empty;
        public static string SaveDir => saveDir ?? string.Empty;
        public static string FilterSettingsPath => filterSettingsPath??string.Empty;
    }
}
