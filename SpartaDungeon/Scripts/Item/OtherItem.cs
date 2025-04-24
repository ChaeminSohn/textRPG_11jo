using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class OtherItem : ITradable    //기타 아이템
    {
        private ItemInfo itemInfo;
        public int ID => itemInfo.ID;
        public string Name => itemInfo.Name;

        public string Description => itemInfo.Description;

        public int Price => itemInfo.Price;

        public ItemType ItemType => ItemType.Other;
        public bool IsSoldOut { get; private set; }
        public int ItemCount { get; private set; }

        public OtherItem(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            IsSoldOut = itemInfo.IsSoldOut;
            ItemCount = itemInfo.ItemCount;
        }
        public ItemInfo GetItemInfo()
        {
            return new ItemInfo(ID, Name, ItemType, Description, Price, IsSoldOut, ItemCount);
        }

        public void OnTrade()
        {
            throw new NotImplementedException();
        }

        public void ChangeItemCount(int change)
        {
            ItemCount += change;
            if (ItemCount == 0)
            {
                IsSoldOut = true;
            }
        }
        public void ShowInfo(bool showPrice)
        {
            string countFormatted = Utils.PadToWidth($"x{ItemCount}", 3);
            string nameFormatted = Utils.PadToWidth(Name, 15);
            string descFormatted = Utils.PadToWidth(Description, 50);
            if (showPrice)
            {   //가격 표시 - 주로 상점에서 이용
                string priceFormatted;
                if (!IsSoldOut)
                {
                    priceFormatted = Utils.PadToWidth($"{Price} G", 8);
                }
                else
                {
                    priceFormatted = Utils.PadToWidth("[판매 완료]", 8);
                }
                Console.WriteLine($"{countFormatted} | {nameFormatted} | {priceFormatted} \n  - {descFormatted}");
            }
            else    //가격 미표시 
            {
                Console.WriteLine($"{countFormatted} | {nameFormatted} \n  - {descFormatted}");
            }
        }

        public OtherItem CloneItem(int itemCount)      //상점, 인벤토리에 전달 시, 새로운 객체를 복사하여 전달
        {
            var clone = new OtherItem(itemInfo);
            clone.ItemCount = itemCount;
            return clone;
        }
    }
}
