using HarmonyLib;
using System.Reflection;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    /// <summary>
    /// 模组初始化检测类
    /// 代码有点烂,但是烂代码出东西
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
