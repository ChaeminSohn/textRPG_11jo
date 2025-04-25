using System.Threading;
using Microsoft.VisualBasic;

namespace SpartaDungeon
{
    internal class Battle
    {
        Player player;  //플레이어
        Monster[] monsters;  //몬스터들
        List<Monster> killedMonsters = new List<Monster>();  //처치한 몬스터 수
        int droppedMeso; //몬스터가 드랍한 메소 총합
        Dictionary<string, int> droppedItemCounts = new Dictionary<string, int>(); //드랍 아이템 정리용 딕셔너리
        bool isPlayerRun = false;   //도망가기 옵션
        bool isPlayerTurn = true;

        public Battle(Player player, Monster[] monsters)
        {
            this.player = player;
            this.monsters = monsters;
        }

        public void StartBattle()
        {
            int startHP = player.CurrentHP;
            int startMP = player.CurrentMP;
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
                Console.WriteLine($"MP {startMP} -> {player.CurrentMP}");
            }
            else
            {
                Console.WriteLine("패배!");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {startHP} -> 0");
                return;
            }
            Utils.Pause(false);

            Console.Clear();
            Console.WriteLine("[사냥 결과]");
            Console.WriteLine($"{droppedMeso} 메소");
            player.ChangeMeso(droppedMeso);
            foreach (var entry in droppedItemCounts)
            {
                Console.WriteLine($"{entry.Key} x {entry.Value}");
            }
            Utils.Pause(true);
        }

        void MyTurnAction() //플레이어 턴
        {
            while (isPlayerTurn)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Battle!\n");
                Console.ResetColor();
                ShowBattleInfo();

                Console.WriteLine("\n1. 공격");
                ColorFont.Write("2. 스킬\n", Color.Blue);
                ColorFont.Write("\n0. 도망가기\n", Color.Magenta);

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
                        break;
                    case 2:
                        PlayerSkillAction();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        continue;
                }
            }
        }

        void PlayerAttackAction(int skillNum) //플레이어 공격 액션
        {
            bool isSkill = skillNum > -1 ? true : false; // skillNum이 -1보다 크다면 스킬을 발동

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Battle!\n");
                Console.ResetColor();
                ShowBattleInfo();
                if (isSkill == true)
                {
                    ColorFont.Write("\n[스킬]", Color.Blue);
                    Console.WriteLine($"\n{player.Skills[skillNum].Name} : {player.Skills[skillNum].Description}");
                }

                Console.WriteLine("\n0. 취소");
                Console.WriteLine("\n대상을 선택해주세요.");

                int playerInput = Utils.GetPlayerInput();

                if (playerInput > monsters.Length || playerInput == -1)   //잘못된 값 입력
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                }
                else if (playerInput == 0)  //취소 선택
                {
                    return;
                }
                else if (monsters[playerInput - 1].IsDead)  //이미 죽은 몬스터 선택
                {
                    Console.WriteLine("이미 사망한 몬스터입니다.");
                    Utils.Pause(false);
                }
                else
                {
                    isPlayerTurn = false;
                    if (isSkill == false)
                    {
                        player.AutoAttack(monsters[playerInput - 1]);
                    }
                    if (isSkill == true)
                    {
                        player.Skills[skillNum].Activate(player, monsters[playerInput - 1], monsters);
                    }
                    Utils.Pause(true);
                    foreach (Monster monster in monsters)
                    {
                        if (monster.IsDead)
                        {
                            {
                                MonsterDead(monster);
                            }
                        }
                    }
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

        private void MonsterDead(Monster monster) //몬스터 처치시 킬카운트 상승 + 전리품 표시
        {
            if (killedMonsters.Contains(monster))   //이미 보상 처리한 몬스터인 경우
            {
                return;
            }
            killedMonsters.Add(monster);
            {
                if (player.monsterKillCounts.ContainsKey(monster.Id))
                    player.monsterKillCounts[monster.Id]++;
                else
                    player.monsterKillCounts[monster.Id] = 1;
            }

            player.GetEXP(monster.ExpReward);       //경험치 획득
            droppedMeso += monster.MesoReward;      //골드 획득 
            Console.Clear();
            Console.WriteLine($"{monster.Name}을(를) 잡았다!");
            Console.WriteLine("[획득 아이템]");
            Console.WriteLine($"{monster.MesoReward} 메소");
            //드랍 아이템 처리
            foreach (ItemInfo itemInfo in Utils.GetDroppedItems(monster.Drops))
            {
                if (droppedItemCounts.ContainsKey(itemInfo.Name))
                {
                    droppedItemCounts[itemInfo.Name]++;
                }
                else
                {
                    droppedItemCounts[itemInfo.Name] = 1;
                }
                player.Inventory.AddItem(itemInfo); //드랍 아이템 획득
                Console.WriteLine($"{itemInfo.Name} x {itemInfo.ItemCount}");
            }
            Utils.Pause(true);
        }

        void EnemyTurnAction()  //몬스터 턴
        {
            Random randSkill = new Random();
            foreach (Monster monster in monsters)
            {
                if (!monster.IsDead)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Battle!");
                    Console.ResetColor();
                    if(randSkill.NextDouble() < 0.5f && 100 < monster.Id) monster.Skills[0].Activate(monster, player, monsters); // 보스면 20%로 스킬공격
                    else monster.AutoAttack(player);
                    Utils.Pause(true);
                }
            }
            isPlayerTurn = true; // 다시 플레이어 턴으로
        }

        void ShowBattleInfo()   //몬스터, 플레이어 정보 표시
        {
            string levelFormatted;
            string nameFormatted;
            string stateFormatted;
            //몬스터 정보 표시
            for (int i = 0; i < monsters.Length; i++)
            {
                levelFormatted = Utils.PadToWidth($"{monsters[i].Level}", 2);
                nameFormatted = Utils.PadToWidth(monsters[i].Name, 12);
                stateFormatted = Utils.PadToWidth(monsters[i].IsDead ? "Dead" : "HP : " + monsters[i].CurrentHP, 10);
                Console.WriteLine($"{i + 1}. Lv{levelFormatted} {nameFormatted} {stateFormatted}");
            }
            Console.WriteLine("\n\n[내정보]");
            Console.WriteLine($"Lv.{player.Level} [{player.Name}] ({Utils.JobDisplayNames[player.Job]})");
            Console.WriteLine($"HP {player.CurrentHP}/{player.FullHP}");
            Console.WriteLine($"MP {player.CurrentMP}/{player.FullMP}");
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

        bool EveryMonsterIsDead()   // 모든 몬스터 죽음 
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