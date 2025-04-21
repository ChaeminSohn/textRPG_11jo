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
        public string Name => itemInfo.Name;
        public string Description => itemInfo.Description;
        public int Price => itemInfo.Price;
        public ItemType ItemType => itemInfo.ItemType;
        public bool IsForSale { get; private set; }   //판매 여부

        public Usable(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            IsForSale = itemInfo.IsForSale;
        }
        public ItemInfo GetItemInfo()
        {
            return new ItemInfo(Name, ItemType, Description, Price, IsForSale);
        }

        public void OnTrade()
        {

        }

        public void ShowInfo(bool showPrice)
        {

        }
        public void OnUse()
        {

        }
    }
}
