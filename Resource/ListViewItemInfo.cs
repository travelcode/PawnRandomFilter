using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nomadicooer.rimworld.prf
{
    internal class ListViewItemInfo<TItem>
    {
        //列表项
        private readonly List<TItem> items = new List<TItem>();
        private readonly List<TItem> initialItem;
        private readonly int maxLineCharCount = 0;
        private readonly string maxCharCountItemResult = string.Empty;
        private readonly int initialCount = 0;
        private readonly Func<TItem, string> callBack;
        private readonly Func<TItem, string> toolTipCallBack;
        private Vector2 scrollPosition = Vector2.zero;
        private TItem curSelected;
        private bool hasSelected = false;
        private Vector2 lineItemSize = Vector2.zero;
        private string title;
        private string toolTip;
        private bool shouldSrollToEnd = false;
        public bool HasSelected => hasSelected;

        public TItem CurSelected
        {
            get => curSelected;
            set
            {
                hasSelected = true;
                curSelected = value;
            }
        }
        public bool Remove(TItem item)
        {
            if (item == null) return false;
            if (item.Equals(CurSelected))
            {
                hasSelected = false;
            }
            return items.Remove(item);
        }
        public string GetItemResult(TItem item)
        {
            return callBack(item);
        }
        public void Add(TItem item)
        {
            items.Add(item);
        }
        public bool MoveTo(ListViewItemInfo<TItem> otherItems)
        {
            if (!hasSelected || otherItems == null || curSelected == null)
            {
                hasSelected = false;
                return false;
            }
            if (Remove(curSelected))
            {
                otherItems.Add(curSelected);
                hasSelected = false;
                return true;
            }
            return false;
        }
        public Vector2 ScrollPosition { get => scrollPosition; set => scrollPosition = value; }

        public int InitialCount => initialCount;

        public int MaxLineCharCount => maxLineCharCount;

        public List<TItem> InitialItem => initialItem;

        public List<TItem> Items => items;

        public string MaxCharCountItemResult => maxCharCountItemResult;

        public string Title { get => title; set => title = value; }
        public string ToolTip { get => toolTip; set => toolTip = value; }
        public Vector2 LineItemSize { get => lineItemSize; set => lineItemSize = value; }
        public bool ShouldSrollToEnd { get => shouldSrollToEnd; set => shouldSrollToEnd = value; }

        public Func<TItem, string> ToolTipCallBack => toolTipCallBack;

        internal ListViewItemInfo(List<TItem> items, Func<TItem, string> callBack, Func<TItem, string> toolTipCallBack) :
        this(items, string.Empty, callBack, toolTipCallBack)
        {

        }
        internal ListViewItemInfo(List<TItem> items, string title, Func<TItem, string> callBack, Func<TItem, string> toolTipCallBack) :
        this(items, title, string.Empty, callBack, toolTipCallBack)
        {

        }
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        internal ListViewItemInfo(List<TItem> items, string title, string toolTip, Func<TItem, string> callBack, Func<TItem, string> toolTipCallBack)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
            this.title = title;
            this.toolTip = toolTip;
            this.callBack = callBack;
            initialCount = items.Count;
            initialItem = items;
            this.toolTipCallBack = toolTipCallBack;
            foreach (var item in items)
            {
                string result = this.callBack == null
                    ? item == null ? string.Empty : item.ToString()
                    : this.callBack(item);
                if (result.Length > MaxLineCharCount)
                {
                    maxCharCountItemResult = result;
                    maxLineCharCount = result.Length;
                }
                this.items.Add(item);
            }
        }

    }

}
