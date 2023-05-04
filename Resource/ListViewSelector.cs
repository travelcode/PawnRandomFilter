using System;
using System.Collections.Generic;

namespace Nomadicooer.rimworld.prf
{
    internal class ListViewSelector<TItem>
    {
        private readonly ListViewItemInfo<TItem> srcItemsInfo;
        private readonly ListViewItemInfo<TItem> dstItemsInfo;
        private readonly Func<TItem, string> callBack;
        public ListViewSelector(List<TItem> srcItems, List<TItem> dstItems, Func<TItem, string> callBack, Func<TItem, string> toolTipCallBack)
        {
            this.callBack = callBack;
            srcItems = FilterItems(srcItems, dstItems);
            srcItemsInfo = new ListViewItemInfo<TItem>(srcItems,
                                                            "SourceItems".LabelText(),
                                                            "SourceItems".TooltipText(),
                                                            callBack,
                                                            toolTipCallBack);
            dstItemsInfo = new ListViewItemInfo<TItem>(dstItems,
                                                            "TargetItems".LabelText(),
                                                            "TargetItems".TooltipText(),
                                                            callBack,
                                                            toolTipCallBack);
        }

        private List<TItem> FilterItems(List<TItem> srcItems, List<TItem> dstItems)
        {
            List<TItem> newSrcItems = new List<TItem>();
            foreach (TItem item in srcItems)
            {
                if (item == null)
                {
                    continue;
                }
                bool r = dstItems.Exists((dstItem) =>
                  {
                      string itemResult = callBack(item);
                      string dstItemResult = callBack(dstItem);
                      return itemResult == dstItemResult;
                  });
                if (!r)
                {
                    newSrcItems.Add(item);
                }
            }
            return newSrcItems;
        }

        internal ListViewItemInfo<TItem> SrcItemsInfo => srcItemsInfo;

        internal ListViewItemInfo<TItem> DstItemsInfo => dstItemsInfo;

        public void Add()
        {
            srcItemsInfo.MoveTo(dstItemsInfo);
        }
        public void Remove()
        {
            dstItemsInfo.MoveTo(srcItemsInfo);
        }
    }
}
