using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class OtherItem : ITradable    //기타 아이템
    {
        private ItemInfo itemInfo;
        public string Name => itemInfo.Name;

        public string Description => itemInfo.Description;

        public int Price => itemInfo.Price;

        public ItemType ItemType => ItemType.Other;

        public bool IsForSale { get; private set; }


        public OtherItem(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            IsForSale = itemInfo.IsForSale;
        }
        public ItemInfo GetItemInfo()
        {
            throw new NotImplementedException();
        }

        public void OnTrade()
        {
            throw new NotImplementedException();
        }

        public void ShowInfo(bool showPrice)
        {

        }
    }
}
