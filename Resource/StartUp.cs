using HarmonyLib;
using System.Reflection;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    /// <summary>
    /// 模组初始化检测类
    /// </summary>
    [StaticConstructorOnStartup]
    public static class StartUp
    {
  

        static StartUp()
        {
            Logger.Level = LoggerLevel.Trace;
            Harmony harmony = new Harmony(ModPathUtility.PackageId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            FilterSettings.Instance.LoadSettings();
        }
    }
}
