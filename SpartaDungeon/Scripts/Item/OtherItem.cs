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
        public int ID => itemInfo.ID;
        public string Name => itemInfo.Name;

        public string Description => itemInfo.Description;

        public int Price => itemInfo.Price;

        public ItemType ItemType => ItemType.Other;
        public bool IsShopItem { get; private set; }
        public bool IsSoldOut { get; private set; }
        public int ItemCount { get; private set; }

        public OtherItem(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            IsShopItem = itemInfo.IsShopItem;
            IsSoldOut = itemInfo.IsSoldOut;
        }
        public ItemInfo GetItemInfo()
        {
            return new ItemInfo(ID, Name, ItemType, Description, Price, IsShopItem, IsSoldOut, ItemCount);
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
