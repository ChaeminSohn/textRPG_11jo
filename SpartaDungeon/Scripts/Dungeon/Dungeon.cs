using System.Linq.Expressions;
using System.Threading;


namespace SpartaDungeon
{
    
        internal class Dungeon
        {
            private Player player;
            private static Random random;
            private List<Monster> monsters;

            public Dungeon(Player player, List<Monster> monsters)
            {
                this.player = player;
                this.monsters = monsters;
                random = new Random();
            }

            public void Enter()
            {
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("던전 입장");
                    Console.ResetColor();
                    Console.WriteLine("난이도를 선택하세요.\n");
                    Console.WriteLine("1. 매우 쉬움");
                    Console.WriteLine("2. 쉬움");
                    Console.WriteLine("3. 보통");
                    Console.WriteLine("4. 어려움");
                    Console.WriteLine("5. 매우 어려움");
                    Console.WriteLine("0. 나가기");

                    int input = Utils.GetPlayerInput();

                    Difficulty selectedDifficulty;
                    switch (input)
                    {
                        case 0:
                            return;
                        case 1:
                            selectedDifficulty = Difficulty.VeryEasy;
                            break;
                        case 2:
                            selectedDifficulty = Difficulty.Easy;
                            break;
                        case 3:
                            selectedDifficulty = Difficulty.Normal;
                            break;
                        case 4:
                            selectedDifficulty = Difficulty.Hard;
                            break;
                        case 5:
                            selectedDifficulty = Difficulty.VeryHard;
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Utils.Pause(false);
                            continue;
                    }

                    int requiredLevel = GetRequiredLevel(selectedDifficulty);
                    if (player.Level < requiredLevel)
                    {
                        Console.WriteLine($"\n※ 이 난이도에 입장하려면 플레이어 레벨이 {requiredLevel} 이상이어야 합니다.");
                        Utils.Pause(false);
                        continue;
                    }

                    StartBattle(GenerateMonsters(selectedDifficulty));
                    return;
                }
            }

            private void StartBattle(List<Monster> monsters)
            {
                Battle battle = new Battle(player, monsters.ToArray());
                battle.StartBattle();
            }

            private List<Monster> GenerateMonsters(Difficulty difficulty)
            {
                List<Monster> allMonsters = monsters;
                List<Monster> selectedMonsters = new List<Monster>();

                int monsterLevel = GetMonsterLevel(difficulty);
                int monsterCount = random.Next(3, 6); // 3~5마리 랜덤

                for (int i = 0; i < monsterCount; i++)
                {
                    int index = random.Next(allMonsters.Count);
                    Monster monster = allMonsters[index].Clone();
                    monster.Level = monsterLevel;
                    selectedMonsters.Add(monster);
                }

                return selectedMonsters;
            }

            private int GetRequiredLevel(Difficulty difficulty)
            {
                return difficulty switch
                {
                    Difficulty.VeryEasy => 1,
                    Difficulty.Easy => 3,
                    Difficulty.Normal => 5,
                    Difficulty.Hard => 7,
                    Difficulty.VeryHard => 9,
                    _ => 1
                };
            }

            private int GetMonsterLevel(Difficulty difficulty)
            {
                return difficulty switch
                {
                    Difficulty.VeryEasy => 1,
                    Difficulty.Easy => 3,
                    Difficulty.Normal => 5,
                    Difficulty.Hard => 7,
                    Difficulty.VeryHard => 9,
                    _ => 1
                };
            }
        }

        public enum Difficulty
        {
            VeryEasy,
            Easy,
            Normal,
            Hard,
            VeryHard
        }
    }
