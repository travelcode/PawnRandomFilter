using System;
using UnityEngine;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    public class AlertPawnSettingsFilter<TWindow> : Window where TWindow : Window
    {
        private const int AlertHeight = 60;
        private Rect curWinRect = Rect.zero;
        private string ShowText = string.Empty;
        private Rect textRect = Rect.zero;
        private readonly int interverSecond = 5;
        private readonly DateTime endTime = DateTime.Now;
        public string Text { get => ShowText; set => ShowText = value; }
        public AlertPawnSettingsFilter()
        {
            doCloseX = false;
            doCloseButton = false;
            doWindowBackground = false;
            grayOutIfOtherDialogOpen=false;
            TWindow window = Find.WindowStack.WindowOfType<TWindow>();
            if (window == null) { return; };
            curWinRect = window.windowRect;
            curWinRect = new Rect(curWinRect.x, curWinRect.y - AlertHeight - 2, curWinRect.width, AlertHeight);
            float totalMargin = 2 * Margin;
            textRect = new Rect(0, 0, curWinRect.width, curWinRect.height);
            endTime = DateTime.Now.AddSeconds(interverSecond);
        }
        public override void WindowOnGUI()
        {
            if (!Find.WindowStack.IsOpen<TWindow>())
            {
                Close(false);
                return;
            };
            windowRect = curWinRect;
            base.WindowOnGUI();
        }
        public override void DoWindowContents(Rect inRect)
        {
            if (!Find.WindowStack.IsOpen<TWindow>())
            {
                Close(false);
                return;
            };
            if (DateTime.Compare(DateTime.Now, endTime) > 0)
            {
                Close(false);
                return;
            };
            Widgets.Label(textRect, ShowText);
        }
    }
}
