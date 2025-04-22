using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class BattleResult
    {
        public int EarnedExp { get; set; }
        public int EarnedGold { get; set; }
        public List<ItemInfo> EarnedItems { get; set; }

        public BattleResult()
        {
            EarnedItems = new List<ItemInfo>();
        }

        public void DisplayResult(Player player)
        {
            Console.WriteLine("\nBattle!! - Result\n");
            Console.WriteLine("Victory\n");
            Console.WriteLine("던전에서 몬스터 3마리를 잡았습니다.\n");
            Console.WriteLine("[캐릭터 정보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.BaseFullHP} -> {player.CurrentHP}\n");

            Console.WriteLine("[획득 아이템]");
            Console.WriteLine($"{EarnedGold} Gold");
            foreach (var item in EarnedItems)
            {
                Console.WriteLine($"{item.Name} - 1");
            }
            Console.WriteLine();
            Console.WriteLine("0. 다음");
        }
    }
}
