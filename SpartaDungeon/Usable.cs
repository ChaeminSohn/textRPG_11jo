using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Usable : ITradable   //소비 아이템
    {
        private ItemInfo itemInfo;
        public int ID => itemInfo.ID;
        public string Name => itemInfo.Name;
        public string Description => itemInfo.Description;
        public int Price => itemInfo.Price;
        public ItemType ItemType => itemInfo.ItemType;
        public bool IsForSale { get; private set; }   //판매 여부
        public int ItemCount { get; private set; } //아이템 개수

        public Usable(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            IsForSale = itemInfo.IsForSale;
        }
        public ItemInfo GetItemInfo()
        {
            return new ItemInfo(ID, Name, ItemType, Description, Price, IsForSale, ItemCount);
        }

        public void OnTrade()
        {

        }

        public void ShowInfo(bool showPrice)
        {

        }
        public void Use()
        {

        }
    }
}
