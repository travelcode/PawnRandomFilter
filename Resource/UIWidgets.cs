using System;
using System.Collections.Generic;
using Nomadicooer.rimworld.prf;
using UnityEngine;
using Verse;

namespace Nomadicooer.rimworld.prf
{
    internal class UIWidgets
    {
        private const int BasicSize = 6;
        private static float TotalHeight = 0;
        private const float LineHeight = BasicSize * 4;
        private const float StartX = BasicSize * 2;
        private const float MarginHeight = BasicSize * 2;
        internal static readonly Vector2 DialogInitSize = new Vector2(600, 900);
        internal static readonly Rect ScrollViewPosition = new Rect(0, 0, DialogInitSize.x - 36, DialogInitSize.y - 115);
        private const float ItemTitleWidthFactor = 0.15f;
        private const int ListViewLines = 5;
        private static float ItemTitleMaxWidth = 0;
        private static bool FirstRun = true;
        internal static Rect ScrollViewContentPostion = GetScrollViewContentPostion();
        private readonly static GUIStyle TextFieldVariantStyle = GetTextFieldVariantStyle();

        internal static void Begin()
        {
            ScrollViewContentPostion = GetScrollViewContentPostion();
            Widgets.DrawHighlight(ScrollViewContentPostion);
            TotalHeight = 0;
        }
        internal static void End()
        {
            FirstRun = false;
        }
        internal static void DrawGroupTitle(string titleKey)
        {
            DrawGroupTitle(titleKey.LabelText(), titleKey.TooltipText());
        }
        internal static void DrawGroupTitle(string title, string tooltip)
        {
            Rect rect = IncrementLine();
            TooltipHandler.TipRegion(rect, tooltip);
            GUI.Label(rect, title, TextFieldVariantStyle);
            Widgets.DrawHighlight(rect);
        }
        internal static int IntField(string titleKey, int value, int multiplier = 1)
        {
            return IntField(titleKey.LabelText(), titleKey.TooltipText(), value, multiplier);
        }
        internal static int IntField(string title, string tooltip, int value, int multiplier = 1)
        {
            Rect itemRect = Preprocesse(title, tooltip);
            string editBuffer = value.ToString();
            Widgets.IntEntry(itemRect, ref value, ref editBuffer, multiplier);
            return value;
        }
        internal static void SaveButton()
        {
            Rect rect = IncrementLine();
            float x = rect.x + (rect.width - 100) / 2;
            Rect buttonArea = new Rect(x, rect.y, 100, rect.height);
            if (Widgets.ButtonText(buttonArea, "Save".Translate()))
            {
                FilterSettings.Instance.Save();
            }
        }
        internal static IEnum RadioButtonGroup<IEnum>(IEnum chosen) where IEnum : Enum
        {
            Type type = typeof(IEnum);
            string titleKey = type.Name;
            string[] labels = type.GetEnumNames();
            int length = labels.Length;
            string[] enumLabels = new string[length];
            IEnum[] enumValues = new IEnum[length];
            int chosenIndex = 0;
            for (int i = 0; i < length; i++)
            {
                enumValues[i] = (IEnum)Enum.Parse(type, labels[i]);
                enumLabels[i] = enumValues[i].Translate();
                if (enumValues[i].Equals(chosen))
                {
                    chosenIndex = i;
                }
            }
            chosenIndex = RadioButtonGroup(titleKey.LabelText(), titleKey.TooltipText(), chosenIndex, enumLabels);
            return enumValues[chosenIndex];
        }
        internal static int RadioButtonGroup(string titleKey, int chosen, params string[] labels)
        {
            return RadioButtonGroup(titleKey.LabelText(), titleKey.TooltipText(), chosen, labels);
        }
        internal static int RadioButtonGroup(string title, string tooltip, int chosen, params string[] labels)
        {
            Rect itemRect = Preprocesse(title, tooltip);
            int length = labels.Length;
            float radioWidth = itemRect.width / length;
            for (int i = 0; i < length; i++)
            {
                Rect radioRect = new Rect(itemRect.x + radioWidth * i, itemRect.y, radioWidth, itemRect.height);
                if (RadioButton(radioRect, labels[i], i == chosen))
                {
                    chosen = i;
                }
            }
            return chosen;
        }
        internal static IntRange IntRangeSlider(string titleKey, IntRange intRange, int min = 0, int max = 100, int minInvterval = 0)
        {
            return IntRangeSlider(titleKey.LabelText(), titleKey.TooltipText(), intRange, min, max, minInvterval);
        }
        internal static IntRange IntRangeSlider(string title, string tooltip, IntRange intRange, int min = 0, int max = 100, int minInvterval = 0)
        {
            Rect rect = Preprocesse(title, tooltip);
            Widgets.IntRange(rect, rect.GetHashCode(), ref intRange, min, max, null, minInvterval);
            return intRange;
        }
        internal static List<TItem> ListViewSelector<TItem>(ListViewSelector<TItem> selector)
        {
            Rect rect = IncrementLine(ListViewLines);
            //绘制左侧的列表
            Rect areaRect = new Rect(rect.x, rect.y, rect.width * 0.4f, rect.height);
            DrawListView(areaRect, selector.SrcItemsInfo, selector.DstItemsInfo);
            //绘制中间按钮
            areaRect = new Rect(areaRect.xMax, areaRect.y, rect.width * 0.2f, areaRect.height);
            DrawButton(areaRect, selector);
            //绘制右边列表
            areaRect = new Rect(areaRect.xMax, areaRect.y, rect.width * 0.4f, areaRect.height);
            DrawListView(areaRect, selector.DstItemsInfo, selector.SrcItemsInfo);
            return selector.DstItemsInfo.Items;
        }
        private static void DrawButton<TItem>(Rect areaRect, ListViewSelector<TItem> selector)
        {
            Vector2 buttonTextSize = Text.CalcSize(">>");
            float startX = areaRect.x + (areaRect.width - buttonTextSize.x * 2f) / 2f;
            float startY = areaRect.y;
            startY += (areaRect.height - 2f * LineHeight - MarginHeight) / 2f;
            Rect buttonRect = new Rect(startX, startY, buttonTextSize.x * 2f, LineHeight);
            bool r = Widgets.ButtonText(buttonRect, ">>");
            if (r)
            {
                selector.Add();
            }
            buttonRect = new Rect(startX, buttonRect.yMax + MarginHeight, buttonTextSize.x * 2f, LineHeight);
            r = Widgets.ButtonText(buttonRect, "<<");
            if (r)
            {
                selector.Remove();
            }

        }
        private static void DrawListView<TItem>(Rect rect, ListViewItemInfo<TItem> itemsInfo, ListViewItemInfo<TItem> otherItemsInfo)
        {
            Widgets.DrawHighlight(rect);
            //绘制标题
            float startY = rect.y;
            startY = DrawListViewTitle(rect, itemsInfo, startY);
            //绘制列表框
            DrawListView(rect, startY, itemsInfo, otherItemsInfo);
        }
        private static void DrawListView<TItem>(Rect rect, float startY, ListViewItemInfo<TItem> itemsInfo, ListViewItemInfo<TItem> otherItemsInfo)
        {
            float ListViewheight = rect.height - (startY - rect.y);
            Rect areaRect = new Rect(rect.x, startY, rect.width, ListViewheight);
            if (itemsInfo.LineItemSize == Vector2.zero)
            {
                float listViewContentWidth = areaRect.width - GUI.skin.verticalScrollbar.fixedWidth * 1.1f;
                itemsInfo.LineItemSize = TextFieldVariantStyle.CalcSize(new GUIContent(itemsInfo.MaxCharCountItemResult));
                float x = Math.Max(listViewContentWidth, itemsInfo.LineItemSize.x);
                itemsInfo.LineItemSize = new Vector2(x, itemsInfo.LineItemSize.y);
            }
            float viewContentHeight = itemsInfo.Items.Count * itemsInfo.LineItemSize.y;
            Rect viewRect = new Rect(rect.x, startY, itemsInfo.LineItemSize.x, viewContentHeight);
            Widgets.DrawHighlight(areaRect);
            itemsInfo.ScrollPosition = GUI.BeginScrollView(areaRect, itemsInfo.ScrollPosition, viewRect, false, true);
            DrawItems(areaRect, viewRect, itemsInfo, otherItemsInfo);
            if (itemsInfo.ShouldSrollToEnd)
            {
                float scrollx = viewRect.x;
                float scrolly = viewRect.height - areaRect.height + viewRect.y;
                Rect scrollRect = new Rect(scrollx, scrolly, viewRect.width, viewRect.height);
                GUI.ScrollTo(scrollRect);
                itemsInfo.ShouldSrollToEnd = false;
            }
            GUI.EndScrollView();
        }
        private static void DrawItems<TItem>(Rect scrollArea, Rect viewRect, ListViewItemInfo<TItem> itemsInfo, ListViewItemInfo<TItem> otherItemInfo)
        {
            List<TItem> items = itemsInfo.Items;
            Rect position = new Rect(viewRect.x, viewRect.y, viewRect.width, itemsInfo.LineItemSize.y);
            TItem needRemoveItem;
            bool needRemove = false;
            foreach (var item in items)
            {
                string result = itemsInfo.GetItemResult(item);
                bool itemClicked = GUI.Button(position, result.Translate(), TextFieldVariantStyle);
                if (itemClicked)
                {
                    itemsInfo.CurSelected = item;
                }
                if (Event.current.clickCount >= 2)
                {
                    needRemoveItem = item;
                    needRemove = true;
                }
                //鼠标同时经过滚动区和选中的列表项目才响应事件,显示提示信息
                if (Mouse.IsOver(position) && InScrollArea(scrollArea, itemsInfo.ScrollPosition))
                {
                    string tip = itemsInfo.ToolTipCallBack(item);
                    TooltipHandler.TipRegion(position, tip);
                }
                if (item != null && itemsInfo.CurSelected != null && itemsInfo.HasSelected && item.Equals(itemsInfo.CurSelected))
                {
                    Widgets.DrawHighlight(position);
                    Widgets.DrawHighlight(position);
                    Widgets.DrawHighlight(position);
                }
                position = new Rect(viewRect.x, position.yMax, viewRect.width, itemsInfo.LineItemSize.y);
            }
            if (needRemove)
            {
                bool r = itemsInfo.MoveTo(otherItemInfo);
                if (r)
                {
                    otherItemInfo.ShouldSrollToEnd = true;
                }
            }
        }
        private static bool InScrollArea(Rect scrollView, Vector2 scrCurPosition)
        {
            Rect newScrollView = new Rect(scrollView.x + scrCurPosition.x, scrollView.y + scrCurPosition.y, scrollView.width, scrollView.height);
            return newScrollView.Contains(Event.current.mousePosition);
        }
        private static float DrawListViewTitle<TItem>(Rect rect, ListViewItemInfo<TItem> itemsInfo, float startY)
        {
            string title = itemsInfo.Title;
            if (!string.IsNullOrEmpty(title))
            {
                Rect areaRect = new Rect(rect.x, rect.y, rect.width, LineHeight);
                Widgets.DrawHighlight(areaRect);
                Label(areaRect, itemsInfo.Title, TextAnchor.MiddleCenter);
                string tooltip = itemsInfo.ToolTip;
                TooltipHandler.TipRegion(areaRect, tooltip);
                startY = areaRect.yMax;
                startY += MarginHeight;
            }

            return startY;
        }
        private static GUIStyle GetTextFieldVariantStyle()
        {
            GUIStyle style = ObjectCloner<GUIStyle, GUIStyle>.Clone(Text.CurTextFieldStyle);
            style.alignment = TextAnchor.MiddleCenter;
            style.border = new RectOffset(0, 0, 2, 2);
            return style;
        }
        private static bool RadioButton(Rect radioRect, string label, bool chosen)
        {
            chosen = Widgets.RadioButton(radioRect.x, radioRect.y, chosen);
            Rect labelRect = radioRect;
            float radioWidth = Widgets.RadioButtonSize + BasicSize;
            labelRect.x += radioWidth;
            labelRect.width = radioRect.width - radioWidth;
            Label(labelRect, label, TextAnchor.MiddleLeft);
            return chosen;
        }
        private static Rect IncrementLine(uint line = 1)
        {
            float maxWidth = ScrollViewContentPostion.width - 2f * StartX;
            Rect rect;
            if (line == 1)
            {
                //单行只返回内容高度,下边距不返回
                rect = new Rect(StartX, TotalHeight, maxWidth, LineHeight);
                TotalHeight += LineHeight + MarginHeight;
                return rect;
            }
            //多行返回区间高度,包括边距但是不包括下边距
            float incrementHeight = line * (LineHeight + MarginHeight);
            incrementHeight -= MarginHeight;
            rect = new Rect(StartX, TotalHeight, maxWidth, incrementHeight);
            incrementHeight += MarginHeight;
            TotalHeight += incrementHeight;
            //TotalLine += line;
            return rect;
        }
        private static Rect GetScrollViewContentPostion()
        {
            float hight = Math.Max(ScrollViewPosition.height, TotalHeight);
            float fiexdWidth = GUI.skin.verticalScrollbar.fixedWidth;
            float width = ScrollViewPosition.width - fiexdWidth * 1.1f;
            Rect rect = new Rect(0, 0, width, hight);
            return rect;
        }
        private static Rect DrawItemTitle(string title, ref Rect rect)
        {
            Rect labelRect = rect;
            if (FirstRun)
            {
                Vector2 titleWidth = Text.CalcSize(title);
                ItemTitleMaxWidth = Math.Max(ItemTitleMaxWidth, titleWidth.x + BasicSize * 4);
                labelRect.width = rect.width * ItemTitleWidthFactor;
            }
            else
            {
                labelRect.width = ItemTitleMaxWidth;
            }
            Label(labelRect, title);
            return labelRect;
        }
        private static void Label(Rect rect, string label, TextAnchor alignment = TextAnchor.MiddleRight)
        {
            Rect position = rect;
            float num = Prefs.UIScale / 2f;
            if (Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > float.Epsilon)
            {
                position.xMin = Widgets.AdjustCoordToUIScalingFloor(rect.xMin);
                position.yMin = Widgets.AdjustCoordToUIScalingFloor(rect.yMin);
                position.xMax = Widgets.AdjustCoordToUIScalingCeil(rect.xMax + 1E-05f);
                position.yMax = Widgets.AdjustCoordToUIScalingCeil(rect.yMax + 1E-05f);
            }
            GUIStyle style = ObjectCloner<GUIStyle, GUIStyle>.Clone(Text.CurFontStyle);
            style.alignment = alignment;
            GUI.Label(position, label, style);
        }
        private static Rect Preprocesse(string title, string tooltip)
        {
            Rect rect = IncrementLine();
            rect.width -= 2 * StartX;
            TooltipHandler.TipRegion(rect, tooltip);
            Rect labelRect = DrawItemTitle(title, ref rect);
            float itemWidth = rect.width - labelRect.width;
            Rect itemRect = new Rect(labelRect.xMax, rect.y, itemWidth, rect.height);
            return itemRect;
        }
        public static void ShowMessage<TWindow>(string message) where TWindow : Window
        {
            AlertPawnSettingsFilter<TWindow> alert = Find.WindowStack.WindowOfType<AlertPawnSettingsFilter<TWindow>>();
            if (alert == null)
            {
                alert = new AlertPawnSettingsFilter<TWindow>();
                Find.WindowStack.Add(alert);
            }
            alert.Text = message;

        }
    }

}