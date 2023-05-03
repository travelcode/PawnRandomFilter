using HarmonyLib;
using System.Reflection;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    /// <summary>
    /// 模组初始化检测类
    /// </summary>
    [StaticConstructorOnStartup]
    internal static class StartUp
    {
        public static string PackageId = "Nomadicooer.Rimworld.CRP";
        static StartUp()
        {
            Logger.Level = LoggerLevel.Trace;
            Harmony harmony = new Harmony(PackageId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
