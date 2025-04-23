namespace SpartaDungeon
{
    [Serializable]
    public class DropTableEntry
    {
        public int itemID;      //아이템 고유번호
        public float DropChance;    //아이템 드랍 확률
        public int MaxQuantity;     //최대 드랍 개수
        public int MinQuantity;     //최소 드랍 개수
    }

    [Serializable]
    public class DropTable
    {
        public List<DropTableEntry> Drops = new List<DropTableEntry>();
    }
}