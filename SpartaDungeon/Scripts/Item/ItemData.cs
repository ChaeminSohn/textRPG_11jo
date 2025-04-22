namespace SpartaDungeon
{
    public class ItemData   //json 파일 아이템 리스트 저장용 클래스
    {
        public List<ItemInfo> Items { get; private set; }

        public List<ItemInfo> Equipments { get; set; } = new List<ItemInfo>();  //장비 아이템 리스트
        public List<ItemInfo> Usables { get; set; } = new List<ItemInfo>();  //소비 아이템 리스트
        public List<ItemInfo> Others { get; set; } = new List<ItemInfo>();  //기타 아이템 리스트

        public ItemData(List<ItemInfo> items)
        {
            Items = items;
            foreach (ItemInfo info in Items)
            {
                switch (info.ItemType)
                {
                    case ItemType.Equipment:
                        Equipments.Add(info);
                        break;
                    case ItemType.Usable:
                        Usables.Add(info);
                        break;
                    case ItemType.Other:
                        Others.Add(info);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}