namespace SpartaDungeon
{
    public class Potion : Usable
    {

        public int HealAmount { get; private set; }
        public Potion(ItemInfo itemInfo) : base(itemInfo)
        {
            HealAmount = itemInfo.HealAmount;
        }

        public override void Use(Player player)

        {
            base.Use(player);
            if (UsableType == UsableType.HealPotion)
            {
                player.RecoverHP(HealAmount);
                Console.WriteLine($"{Name}를 사용했습니다. 체력을 {HealAmount} 회복했습니다.");
            }
            else if (UsableType == UsableType.ManaPotion)
            {
                player.RecoverMP(HealAmount);
                Console.WriteLine($"{Name}를 사용했습니다. 마나를 {HealAmount} 회복했습니다.");
            }
        }

        public override ItemInfo GetItemInfo()
        {
            return new ItemInfo(ID, Name, ItemType, UsableType, Description, Price, IsSoldOut, ItemCount, HealAmount);
        }

        public override Usable CloneItem(int itemCount)
        {
            var clone = new Potion(itemInfo);
            clone.ItemCount = itemCount;
            return clone;
        }
    }
}

