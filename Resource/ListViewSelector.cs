using System;
using System.Collections.Generic;

namespace Nomadicooer.rimworld.crp
{
    internal class ListViewSelector<TItem>
    {
        private readonly ListViewItemInfo<TItem> srcItemsInfo;
        private readonly ListViewItemInfo<TItem> dstItemsInfo;
        private readonly Func<TItem, string> callBack;
        public ListViewSelector(List<TItem> srcItems, List<TItem> dstItems, Func<TItem, string> callBack, Func<TItem, string>toolTipCallBack)
        {
            this.callBack = callBack;
            srcItems = FilterItems(srcItems, dstItems);
            this.srcItemsInfo = new ListViewItemInfo<TItem>(srcItems,
                                                            "SourceItems".LabelText(),
                                                            "SourceItems".TooltipText(),
                                                            callBack,
                                                            toolTipCallBack);
            this.dstItemsInfo = new ListViewItemInfo<TItem>(dstItems,
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
                if (item==null) {
                    continue;
                }
              bool r=  dstItems.Exists((dstItem) =>
                {
                    string itemResult = this.callBack(item);
                    string dstItemResult = this.callBack(dstItem);
                    return itemResult == dstItemResult;
                });
                if (!r) { 
                    newSrcItems.Add(item);
                }
            }
            return newSrcItems;
        }

        internal ListViewItemInfo<TItem> SrcItemsInfo => this.srcItemsInfo;

        internal ListViewItemInfo<TItem> DstItemsInfo => this.dstItemsInfo;

        public void Add()
        {
            this.srcItemsInfo.MoveTo(this.dstItemsInfo);
        }
        public void Remove()
        {
            this.dstItemsInfo.MoveTo(this.srcItemsInfo);
        }
    }
}
