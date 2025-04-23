using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Scripts.Item
{
    internal class Potion
    {
        static void Main()
        {
            int health = 70; //현재체력
            int maxHealth = 100; //최대체력
            int potionCount = 3; // 가지고 있는 포션 개수
            bool running = true; // 프로그램 실행 여부 (루프 컨트롤용)

            while (running) //running 이 true인 동안 계속 반복실행
            {
                Console.Clear();
                Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {potionCount})");
                Console.WriteLine();
                Console.WriteLine("1. 사용하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요.\n >>");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        if (potionCount > 0)
                        {
                            int healAmount = 30;
                            int actualHeal = Math.Min(healAmount, maxHealth - health);
                            health += actualHeal;
                            potionCount--;
                            Console.WriteLine($"\n회복을 완료했습니다! 체력이 {actualHeal}만큼 회복되었습니다.");
                        }
                        else
                        {
                            Console.WriteLine("\n포션이 부족합니다!");
                        }
                        break;

                    case "0":
                        running = false;
                        Console.WriteLine("\n게임을 종료합니다.");
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
