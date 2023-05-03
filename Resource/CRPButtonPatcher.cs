using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    [HarmonyPatch(typeof(Page_ConfigureStartingPawns), "DoWindowContents", new Type[] { typeof(Rect) })]
    internal class CRPButtonPatcher
    {
        //这儿的__instance变量名不能更改
        [HarmonyPostfix]
        public static void Postfix(Page_ConfigureStartingPawns __instance, ref Rect rect)
        {
            DrawButton(__instance, rect);
        }
        private static void DrawButton(Page_ConfigureStartingPawns __instance, Rect rect)
        {
            Rect postion = new Rect(rect.width - 339, rect.y + 21, 106, 102);
            Widgets.DrawHighlight(postion);
            GUI.BeginGroup(postion);
            //绘制条件随机按钮
            postion = new Rect(6, 6, 100, 30);
            string filterButton = PawnFilter.Instance.Running ? "EndFilter" : "StartFilter";
            bool r = Widgets.ButtonText(postion, filterButton.ButtonText(), true, true, true);
            if (r)
            {
                if (!PawnFilter.Instance.Running)
                {
                    PawnFilter.Instance.DoFilter(__instance);
                }
                else
                {
                    Find.WindowStack.Add(new DialogFliterMessage());
                    PawnFilter.Instance.Stop();
                }
            }
            //绘制条件设置按钮
            postion = new Rect(6, 41, 100, 30);
            r = Widgets.ButtonText(postion, "Settings".ButtonText(), true, true, true);
            if (r)
            {
                DialogFliterMessage window = Find.WindowStack.WindowOfType<DialogFliterMessage>();
                window?.Close();
                Find.WindowStack.Add(new DialogSettings());
            }
            GUI.EndGroup();
        }
    }
}