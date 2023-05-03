using RimWorld;
using UnityEngine;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    public class DialogFliterMessage : Window
    {
        private Rect curWinRect = Rect.zero;
        public DialogFliterMessage()
        {
            //添加关闭X按钮
            doCloseX = true;
            Page_ConfigureStartingPawns page_ConfigureStartingPawns = Find.WindowStack.WindowOfType<Page_ConfigureStartingPawns>();
            if (page_ConfigureStartingPawns != null)
            {
                curWinRect = page_ConfigureStartingPawns.windowRect;
                curWinRect = new Rect(curWinRect.x, curWinRect.y - 102, curWinRect.width, 100);
            }
        }
        public override void WindowOnGUI()
        {
            bool r = Find.WindowStack.IsOpen<Page_ConfigureStartingPawns>();
            if (!r) {
                Close(false);
                return;
            }
            windowRect = curWinRect;
            base.WindowOnGUI();
        }

        public override void DoWindowContents(Rect inRect)
        {

        }

    }
}
