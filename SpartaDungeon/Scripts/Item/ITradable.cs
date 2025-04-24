using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum ItemType
    {
        Equipment,  //장비 아이템
        Usable,     //소비 아이템
        Other //기타 아이템
    }
    public interface ITradable //거래 가능한 아이템 구현 시 사용
    {
        int ID { get; }
        String Name { get; }
        String Description { get; }
        int Price { get; }
        ItemType ItemType { get; }
        bool IsSoldOut { get; }     //품절 상태인가?
        public void ShowInfo(bool ShowPrice);
        public void OnTrade();
        public ItemInfo GetItemInfo();
    }
}
