using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Usable : ITradable   //소비 아이템
    {
        private ItemInfo itemInfo;
        public int ID => itemInfo.ID;
        public string Name => itemInfo.Name;
        public string Description => itemInfo.Description;
        public int Price => itemInfo.Price;
        public ItemType ItemType => itemInfo.ItemType;
        public bool IsSoldOut { get; private set; }   //판매 여부
        public int ItemCount { get; private set; } //아이템 개수

        public Usable(ItemInfo itemInfo)
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
            if (!IsSoldOut)
            {
                if (--ItemCount == 0)
                {
                    IsSoldOut = true;
                }

            }
            else
            {

            }
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
            string countFormatted = Utils.PadToWidth($"{ItemCount}개", 5);
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
                Console.WriteLine($"{countFormatted} | {nameFormatted} | {descFormatted} | {priceFormatted}");
            }
            else    //가격 미표시 
            {
                Console.WriteLine($"{countFormatted} | {nameFormatted} | {descFormatted}");
            }
        }
        public virtual void Use()
        {
            ChangeItemCount(-1);
        }
        public Usable CloneItem(int itemCount)      //상점, 인벤토리에 전달 시, 새로운 객체를 복사하여 전달
        {
            var clone = new Usable(itemInfo);
            clone.ItemCount = itemCount;
            return clone;
        }
    }
}
