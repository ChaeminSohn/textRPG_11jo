namespace SpartaDungeon
{
    [Serializable]
    public class DropTableEntry
    {
        public int ItemID { get; set; }      //아이템 고유번호
        public float DropChance { get; set; }      //아이템 드랍 확률
        public int MinQuantity { get; set; }       //최소 드랍 개수
        public int MaxQuantity { get; set; }       //최대 드랍 개수
    }

    [Serializable]
    public class DropTable
    {
        public List<DropTableEntry> Drops = new List<DropTableEntry>();
    }
}