﻿using RimWorld;
using UnityEngine;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    public class DialogFliterMessage : Window
    {
        private const float Speed = 1f;
        private const int AlertHeight = 60;
        private Rect curWinRect = Rect.zero;
        private string ShowText = string.Empty;
        private Rect textRect = Rect.zero;
        public string Text { get => ShowText; set => ShowText = value; }

        public DialogFliterMessage()
        {
            //添加关闭X按钮
            doCloseX = true;
            Page_ConfigureStartingPawns page_ConfigureStartingPawns = Find.WindowStack.WindowOfType<Page_ConfigureStartingPawns>();
            if (page_ConfigureStartingPawns == null)
            {
                return;
            }
            curWinRect = page_ConfigureStartingPawns.windowRect;
            curWinRect = new Rect(curWinRect.x, curWinRect.y - AlertHeight - 2, curWinRect.width, AlertHeight);
            float totalMargin = 2 * Margin;
            float width = curWinRect.width - totalMargin;
            //将文本开始位置设置到最右边
            textRect = new Rect(width, 0, width, curWinRect.height - totalMargin);

        }
        public override void WindowOnGUI()
        {
            if (!Find.WindowStack.IsOpen<Page_ConfigureStartingPawns>())
            {
                Close(false);
                return;
            };
            windowRect = curWinRect;
            base.WindowOnGUI();
        }
        public override void DoWindowContents(Rect inRect)
        {
            if (!Find.WindowStack.IsOpen<Page_ConfigureStartingPawns>())
            {
                Close(false);
                return;
            };
            textRect.x -= Speed;
            Vector2 textSize = Verse.Text.CalcSize(ShowText);
            if (textRect.x + textSize.x < 0)
            {
                Close(false);
                return;
            }
            Widgets.Label(textRect, ShowText);
        }

    }
}
