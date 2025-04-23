using System.Threading;
using Microsoft.VisualBasic;

namespace SpartaDungeon
{
    internal class Battle
    {
        Player player;  //플레이어
        Monster[] monsters;  //몬스터들
        List<Monster> killedMonsters = new List<Monster>();  //처치한 몬스터 수
        List<ITradable> droppedItems = new List<ITradable>(); //드랍 아이템 모음

        bool isPlayerRun = false;   //도망가기 옵션
        public Battle(Player player, Monster[] monsters)
        {
            this.player = player;
            this.monsters = monsters;
        }

        public void StartBattle()
        {
            int startHP = player.CurrentHP;
            while (true)
            {
                MyTurnAction();
                if (EveryMonsterIsDead() || isPlayerRun)
                {
                    break;
                }
                EnemyTurnAction();
                if (player.IsDead)
                {
                    break;
                }
            }
            //배틀 결과 표시
            Console.Clear();
            Console.WriteLine("Battle!! - Result\n");
            if (!player.IsDead)
            {
                Console.WriteLine($"{(isPlayerRun ? "도망쳤습니다." : "승리!")}");
                Console.WriteLine($"\n던전에서 몬스터 {killedMonsters.Count}마리를 잡았습니다.");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {startHP} -> {player.CurrentHP}");
            }
            else
            {
                Console.WriteLine("패배!");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {startHP} -> 0");
                return;
            }
            Utils.Pause(true);

            Dictionary<string, int> itemCounts = new Dictionary<string, int>(); //드랍 아이템 정리용 딕셔너리

            foreach (Monster monster in killedMonsters) //처치한 몬스터 경험치, 드랍 아이템 정리
            {
                player.GetEXP(monster.ExpReward);

                //몬스터 드랍 테이블에서 랜덤 아이템 추출
                foreach (ItemInfo itemInfo in Utils.GetDroppedItems(monster.DropTable))
                {
                    if (itemCounts.ContainsKey(itemInfo.Name))
                    {
                        itemCounts[itemInfo.Name]++;
                    }
                    else
                    {
                        itemCounts[itemInfo.Name] = 1;
                    }
                    player.Inventory.AddItem(ItemFactory.CreateItem(itemInfo));
                }
            }
            Console.Clear();
            Console.WriteLine("[획득 아이템]");
            foreach (var entry in itemCounts)
            {
                Console.WriteLine($"{entry.Key} x {entry.Value}");
            }

        }

        void MyTurnAction() //플레이어 턴
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Battle!\n");
                Console.ResetColor();
                ShowBattleInfo();

                Console.WriteLine("\n1. 공격");
                Console.WriteLine("2. 스킬");
                Console.WriteLine("\n0. 도망가기");

                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 0:     //도망 시도
                        isPlayerRun = TryRun();
                        Console.WriteLine($"\n{(isPlayerRun ? "도망 성공" : "도망 실패")}");
                        Utils.Pause(false);
                        return;
                    case 1:
                        PlayerAttackAction(-1);
                        return;
                    case 2:
                        PlayerSkillAction();
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        continue;
                }
            }
        }

        void PlayerAttackAction(int skillNum) //플레이어 공격 액션
        {
            bool isSkill = skillNum > -1 ? true : false;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Battle!\n");
                Console.ResetColor();
                ShowBattleInfo();
                if (isSkill == true) Console.WriteLine($"\n[스킬] \n {player.Skills[skillNum].Name} : {player.Skills[skillNum].Description}");
                Console.WriteLine("\n0. 취소");
                Console.WriteLine("\n대상을 선택해주세요.");

                int playerInput = Utils.GetPlayerInput();

                if (playerInput > monsters.Length || playerInput == -1)   //잘못된 값 입력
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }
                else if (playerInput == 0)  //취소 선택
                {
                    return;
                }
                else if (monsters[playerInput - 1].IsDead)  //이미 죽은 몬스터 선택
                {
                    Console.WriteLine("이미 사망한 몬스터입니다.");
                    Utils.Pause(false);
                    continue;
                }
                else
                {
                    DealDamage(player, monsters[playerInput - 1], isSkill, skillNum);
                    MonsterDead(playerInput);
                    Utils.Pause(true);
                    return;
                }
            }
        }

        void PlayerSkillAction()  //플레이어 스킬 액션
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Battle!\n");
                Console.ResetColor();
                ShowBattleInfo();
                player.ShowSkillList();
                Console.WriteLine("\n0. 취소");
                Console.WriteLine("\n스킬을 선택해주세요.");

                int skillNumInput = Utils.GetPlayerInput();

                if (skillNumInput > player.Skills.Count || skillNumInput == -1)   //잘못된 값 입력
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }
                else if (skillNumInput == 0)  //취소 선택
                {
                    return;
                }
                else
                {
                    PlayerAttackAction(skillNumInput - 1);
                    return;
                }
            }
        }

        private void MonsterDead(int playerInput)
        {
            if (monsters[playerInput - 1].IsDead)
            {   //몬스터 처치 리스트에 추가
                killedMonsters.Add(monsters[playerInput - 1]);
                {
                    if (player.monsterKillCounts.ContainsKey(monsters[playerInput - 1].Id))
                        player.monsterKillCounts[monsters[playerInput - 1].Id]++;
                    else
                        player.monsterKillCounts[monsters[playerInput - 1].Id] = 1;
                }
            }
        }

        void EnemyTurnAction()  //몬스터 턴
        {
            foreach (IBattleUnit monster in monsters)
            {
                if (!monster.IsDead)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Battle!");
                    Console.ResetColor();
                    DealDamage(monster, player, false , 0);
                    Utils.Pause(true);
                }
            }
        }

        void DealDamage(IBattleUnit attacker, IBattleUnit defender, bool isSkill , int skillNum) //데미지 처리 과정
        {
            Random rand = new Random();
            int baseDamage; // 데미지
            Console.WriteLine($"\n\nLv.{attacker.Level} {attacker.Name} 의 공격!");

            if (rand.NextDouble() < defender.EvadeChance)   //회피 판정
            {
                Console.WriteLine($"Lv.{defender.Level} {defender.Name} 이(가) 공격을 회피했습니다!");
                return;
            }

            int currentHP = defender.CurrentHP;     //피격자의 현재 체력
            int damageVariance = (int)(attacker.Attack * 0.1); // 데미지 편차

            bool isCritical = rand.NextDouble() < attacker.CritChance;  //치명타 판정
            if (isSkill == false) baseDamage = rand.Next(attacker.Attack - damageVariance, attacker.Attack + damageVariance + 1); // 기본 공격 데미지
            else // 스킬 데미지
            {
                int skillDamage = player.Skills[skillNum].Damage + attacker.Attack;
                baseDamage = rand.Next(skillDamage - damageVariance, skillDamage + damageVariance + 1);
            }

            int finalDamage = isCritical ? (int)(baseDamage * 1.5f) : baseDamage;   //최종 데미지

            defender.OnDamage(finalDamage); //데미지 처리

            if (isSkill == false) Console.WriteLine($"Lv.{defender.Level} {defender.Name} 을(를) 맞췄습니다."); // 기본 공격
            else
            {
                Console.WriteLine($"[스킬] : {player.Skills[skillNum].Name}");
                Console.WriteLine($"Lv.{defender.Level} {defender.Name} 을(를) 맞췄습니다.");
            }

            if (isCritical)
            {
                Console.WriteLine("[치명타!] 데미지가 50% 증가했습니다.");
            }

            Console.WriteLine($"\nLv.{defender.Level} {defender.Name}");
            Console.WriteLine($"HP {currentHP} -> {(defender.IsDead ? "Dead" : defender.CurrentHP)}");
        }

        void ShowBattleInfo()   //몬스터, 플레이어 정보 표시
        {
            //몬스터 정보 표시
            for (int i = 0; i < monsters.Length; i++)
            {
                Console.WriteLine($"{i + 1}. Lv{monsters[i].Level} {monsters[i].Name}   {(monsters[i].IsDead ? "Dead" : monsters[i].CurrentHP)} {(monsters[i].IsDead ? " " : monsters[i].Attack)}");
            }
            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.CurrentHP}/{player.FullHP}");
        }

        bool TryRun()       //도주 시도
        {
            Random rand = new Random();

            //도망 성공률은 플레이어의 회피율 영향을 받음
            if (rand.NextDouble() < 0.4f + player.EvadeChance)
            {
                return true;
            }
            return false;
        }

        bool EveryMonsterIsDead()   //몬스터를 
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