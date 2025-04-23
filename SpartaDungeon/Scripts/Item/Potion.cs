using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Potion
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int HealAmount { get; set; }

        int health = 70;
        int maxHealth = 100;

        public Potion(string name, string description, int healAmount)
        {
            Name = name;
            Description = description;
            HealAmount = healAmount;
        }

        public void ShowPotion()
        {
            Console.WriteLine($"{Name} - {Description} (회복량: {HealAmount})");
        }
        public void Use()
        {
            Console.WriteLine($"{Name}를 사용했습니다. 체력을 {HealAmount} 회복했습니다.");
        }
        public void ShowInfo(int index)
        {
            Console.WriteLine($"{index + 1}. {Name} \t 체력을 {HealAmount} 회복시킨다.");
            Console.WriteLine($"   {Description}");
        }

    }


    class PotionInfo
    {
        public void PotionList()
        {
            int health = 70;
            int maxHealth = 100;


            List<Potion> potions = new List<Potion>()
            {
                new Potion("로저의 사과" , "빨갛게 잘 익은 사과이다.", 30),
                new Potion("대추", "빨갛게 잘 익은 대추다.", 30),
                new Potion("레일리의 감", "주황빛이 나는 잘 익은 감이다.", 30),
                new Potion("정의의 꿀밤", "알이 크고 맛있는 밤이다.", 30),

            };

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다.");
                Console.WriteLine($"\n현재 체력: {health}/{maxHealth}");
                Console.WriteLine("\n1. 포션사용");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요.\n >>");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("\n<포션 목록>");
                        for (int i = 0; i < potions.Count; i++)
                        {
                            potions[i].ShowInfo(i);
                        }

                        Console.WriteLine("\n 사용할 포션 번호를 입력하세요:");
                        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= potions.Count)
                        {
                            Potion selectedPotion = potions[choice - 1];
                            int actualHeal = Math.Min(selectedPotion.HealAmount, maxHealth - health);
                            health += actualHeal;
                            selectedPotion.Use();
                            Console.WriteLine($"\n{actualHeal}만큼 회복되었습니다! 현재 체력: {health}");
                        }
                        else
                        {
                            Console.WriteLine("잘못된 선택입니다.");
                        }
                        break;


                    case "0":
                        running = false;
                        Console.WriteLine("\n 게임을 종료합니다.");
                        break;

                    default:
                        Console.WriteLine("\n잘못된 입력입니다. 다시 선택해주세요.");
                        break;

                }

                if (running)
                {

                    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                }
            }
        }
    }
}
