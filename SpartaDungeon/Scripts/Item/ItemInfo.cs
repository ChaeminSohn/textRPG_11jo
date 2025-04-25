using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SpartaDungeon
{
    [Serializable]
    public struct ItemInfo    //아이템 정보를 관리하는 구조체
    {
        //공용 필드
        public int ID { get; set; } //고유번호
        public string Name { get; set; }    //이름
        public string Description { get; set; } //설명
        public int Price { get; set; }  //가격
        public ItemType ItemType { get; set; }  //아이템 종류 (장비, 소비, 기타)
        public bool IsSoldOut { get; set; } = false; //판매 여부

        //장비 전용 필드
        public EquipType EquipType { get; set; } = default; //장착 종류(방어구, 무기)
        public Stat Stat { get; set; } = default;   // 스탯 종류(공격력, 방어력, 체력)
        public float StatValue { get; set; } = -1;    // 스탯 값
        public bool IsEquipped { get; set; } = false; //플레이어 착용 여부

        //소비, 기타 전용 필드
        public UsableType UsableType { get; set; } = default;    //소비 아이템 종류
        public int ItemCount { get; set; } = 0;  //아이템 개수
        public int HealAmount { get; set; } = 0;   //포션의 체력 회복량

        //장비 아이템 생성자
        public ItemInfo(int id, string name, ItemType itemType, EquipType equipType, Stat stat, float statValue, string description, int price,
             bool isSoldOut, bool isEquipped)
        {
            ID = id;
            Name = name;
            EquipType = equipType;
            Stat = stat;
            StatValue = statValue;
            Description = description;
            Price = price;
            ItemType = itemType;
            IsSoldOut = isSoldOut;
            IsEquipped = isEquipped;
        }

        //소비 아이템 생성자
        public ItemInfo(int id, string name, ItemType itemType, UsableType usableType, string description, int price, bool isSoldOut, int itemCount,
            int healAmount)
        {
            ID = id;
            Name = name;
            Description = description;
            Price = price;
            ItemType = itemType;
            UsableType = usableType;
            IsSoldOut = isSoldOut;
            ItemCount = itemCount;
            HealAmount = healAmount;
        }

        //기타 아이템 생성자
        public ItemInfo(int id, string name, ItemType itemType, string description, int price, bool isSoldOut, int itemCount)
        {
            ID = id;
            Name = name;
            Description = description;
            Price = price;
            ItemType = itemType;
            IsSoldOut = isSoldOut;
            ItemCount = itemCount;
        }
    }
}
