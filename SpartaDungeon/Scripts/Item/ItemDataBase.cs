namespace SpartaDungeon
{
    public static class ItemDataBase  //json 파일 아이템 리스트 저장용 클래스
    {
        public static List<ItemInfo> Items { get; private set; }

        public static List<ItemInfo> Equipments { get; set; } = new List<ItemInfo>();  //장비 아이템 리스트
        public static List<ItemInfo> Usables { get; set; } = new List<ItemInfo>();  //소비 아이템 리스트
        public static List<ItemInfo> Others { get; set; } = new List<ItemInfo>();  //기타 아이템 리스트

        public static void Load(string path)
        {
            if (!ConfigLoader.TryLoad<List<ItemInfo>>(path, out var config))
            {
                Console.WriteLine("아이템 데이터 파일이 존재하지 않습니다.");
                Utils.Pause(false);
                Items = new List<ItemInfo>();
                return;
            }
            Items = config;
        }
    }

    //ItemInfo를 기반으로 아이템 객체를 만들어주는 기능 제공
    public static class ItemFactory
    {
        public static ITradable CreateItem(ItemInfo info)
        {
            return info.ItemType switch
            {
                ItemType.Equipment => new Equipment(info),
                ItemType.Usable => new Usable(info),
                ItemType.Other => new OtherItem(info),
                _ => throw new ArgumentException($"지원하지 않는 아이템 타입입니다: {info.ItemType}")
            };
        }
    }


}