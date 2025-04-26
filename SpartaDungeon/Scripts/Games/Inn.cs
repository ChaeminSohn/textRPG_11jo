namespace SpartaDungeon
{
    internal class Inn       //여관
    {
        public void EnterInn(Player player)     // 5. 휴식 액션
        {
            while (true)
            {
                int health = player.CurrentHP;
                int mana = player.CurrentMP;
                Console.Clear();
                Console.WriteLine("<휴식하기>");
                Console.WriteLine($"500 메소를 내면 체력과 마나를 전부 회복할 수 있습니다." +
                    $" (보유 메소 : {player.Meso} 메소)");
                Console.WriteLine("\n1. 휴식하기");
                ColorFont.Write("\n0. 나가기\n", Color.Magenta);
                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        if (player.Meso >= 500)
                        {
                            player.RecoverHP(player.FullHP);
                            player.RecoverMP(player.FullMP);
                            player.ChangeMeso(-500);
                            Console.WriteLine("\n푹 쉬었습니다.");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Console.WriteLine($"마나 {mana} -> {player.CurrentMP}");
                            Utils.Pause(true);
                        }
                        else
                        {
                            Console.WriteLine("\n메소가 부족합니다.");
                            Utils.Pause(false);
                        }
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }
    }
}