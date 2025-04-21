namespace SpartaDungeon
{
    internal class Battle
    {
        Player player;  //플레이어
        MonsterInfo[] monsters;  //몬스터들
        public Battle(Player player, MonsterInfo[] monsters)
        {
            this.player = player;
            this.monsters = monsters;
        }

        public void StartBattle()
        {
            while (true)
            {
                MyTurnAction();
                if (EveryMonsterIsDead())
                {
                    break;
                }
                EnemyTurnAction();
                if (player.IsDead)
                {
                    break;
                }
            }
        }

        void MyTurnAction()     //플레이어 턴
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Battle!");
                Console.ResetColor();
                ShowBattleInfo();

                Console.WriteLine("\n1. 공격");

                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 1:
                        PlayerAttackAction();
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        continue;

                }
            }
        }

        void PlayerAttackAction()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Battle!");
                Console.ResetColor();
                ShowBattleInfo();

                Console.WriteLine("\n0. 취소");
                Console.WriteLine("\n대상을 선택해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 1:
                        //DealDamage(player, monster);
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        continue;
                }
            }
        }

        void EnemyTurnAction()      //몬스터 턴
        {
            foreach (IBattleUnit monster in monsters)
            {
                if (!monster.IsDead)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Battle!");
                    Console.ResetColor();
                    DealDamage(monster, player);
                }
                Utils.Pause(true);
            }
        }
        void DealDamage(IBattleUnit attacker, IBattleUnit defender) //데미지 처리 과정
        {
            Random rand = new Random();
            int currentHP = defender.CurrentHP;     //피격자의 현재 체력
            int damageVariance = (int)(attacker.Attack * 0.1);
            defender.OnDamage(rand.Next(attacker.Attack - damageVariance,
            attacker.Attack + damageVariance));
            Console.WriteLine($"\n\nLv.{attacker.Level} {attacker.Name} 의 공격!");
            Console.WriteLine($"Lv.{defender.Level} {player.Name} 을(를) 맞췄습니다.");
            Console.WriteLine($"\nLv.{defender.Level} {player.Name}");

            if (defender.IsDead)
            {
                Console.WriteLine($"HP {currentHP}-> Dead");
            }
            else
            {
                Console.WriteLine($"HP {currentHP}-> {defender.CurrentHP}");
            }
        }

        void ShowBattleInfo()   //몬스터, 플레이어 정보 표시
        {
            //몬스터 정보 표시
            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.CurrentHP}/{player.FullHP}");

        }

        bool EveryMonsterIsDead()
        {
            foreach (Monster monster in monsters)
            {
                if (!monster.IsDead)
                {
                    return false;
                }
            }
            return true;
        }
    }
}