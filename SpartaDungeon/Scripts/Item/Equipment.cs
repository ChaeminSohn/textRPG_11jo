using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum EquipType
    {
        Weapon = 0,
        Armor = 1
    }
    internal class Equipment : ITradable  //장비 아이템을 구현하는 클래스
    {
        private ItemInfo itemInfo;
        public string Name => itemInfo.Name;
        public string Description => itemInfo.Description;
        public int Price => itemInfo.Price;
        public ItemType ItemType => itemInfo.ItemType;
        public EquipType EquipType => itemInfo.EquipType;
        public Stat Stat => itemInfo.Stat;
        public int StatValue => itemInfo.StatValue;
        public bool IsEquipped { get; private set; }
        public bool IsForSale { get; private set; }

        public Equipment(ItemInfo itemInfo)
        {
            this.itemInfo = itemInfo;
            IsEquipped = itemInfo.IsEquipped;
            IsForSale = itemInfo.IsForSale;
        }

        public void OnTrade()   //상점에서 구매/판매 시 호출
        {
            if (IsForSale)
            {
                IsForSale = false;
            }
            else
            {
                UnEquip();
                IsForSale = true;
            }
        }

        public void ShowInfo(bool showPrice)     //정보 표시 - 가격은 보여줄지 말지 선택
        {
            string typeFormatted = Utils.PadToWidth(Utils.EquipTypeDisplayNames[EquipType], 6);
            string nameFormatted = Utils.PadToWidth(Name, 15);
            string statFormatted = Utils.PadToWidth($"{Utils.StatDisplayNames[Stat]} +{StatValue}", 15);
            string descFormatted = Utils.PadToWidth(Description, 50);
            if (showPrice)
            {   //가격 표시 - 주로 상점에서 이용
                string priceFormatted;
                if (IsForSale)
                {
                    priceFormatted = Utils.PadToWidth($"{Price} G", 8);
                }
                else
                {
                    priceFormatted = Utils.PadToWidth("[판매 완료]", 8);
                }
                Console.WriteLine($"{typeFormatted} | {nameFormatted} | {statFormatted} | {descFormatted} | {priceFormatted}");
            }
            else    //가격 미표시 
            {
                Console.WriteLine($"{typeFormatted} | {nameFormatted} | {statFormatted} | {descFormatted}");
            }
        }

        public ItemInfo GetItemInfo()   //아이템 정보 추출
        {
            return new ItemInfo(Name, ItemType, EquipType, Stat, StatValue, Description, Price, IsForSale, IsEquipped);
        }
        public void Equip()     //아이템 장착
        {
            IsEquipped = true;
        }

        public void UnEquip()   //아이템 장착 해제
        {
            IsEquipped = false;
        }
    }
}
