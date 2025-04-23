using System.Linq.Expressions;
using System.Threading;


namespace SpartaDungeon
{
    internal class Dungeon
    {
        private Player player;
        private static Random random;
        private List<Monster> monsters;
        private readonly Dictionary<Difficulty, string> dungeonIntroductions = new Dictionary<Difficulty, string>
        {
            { Difficulty.VeryEasy, "버섯...좋아하세요?!" },
            { Difficulty.Easy,     "개발하다보니 슬라임을 만들고 있는 건에 대하여..." },
            { Difficulty.Normal,   "문어와 유령의 공통점은?? '두발'로 설 수 없습니다. 탈모죠..." },
            { Difficulty.Hard,     "돼지와 나무...꾼?" },
            { Difficulty.VeryHard, "템은 다 맞추고 오시는거 맞죠? 못 잡으실거에요ㅎㅎ" }
        };

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
                Console.WriteLine("사냥터 입장");
                Console.ResetColor();
                Console.WriteLine("원하시는 곳을 선택하세요.\n");
                Console.WriteLine("1. 헤네시스");
                Console.WriteLine("2. 엘리니아");
                Console.WriteLine("3. 컨닝시티");
                Console.WriteLine("4. 페리온");
                Console.WriteLine("5. 슬리피우드");
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

                bool beginBattle = ShowDungeonIntro(selectedDifficulty);
                if (!beginBattle)
                {
                    continue;
                }

                StartBattle(GenerateMonsters(selectedDifficulty));
                return;
            }
        }

       
        private bool ShowDungeonIntro(Difficulty difficulty)
        {
         
            int minLevel = difficulty switch
            {
                Difficulty.VeryEasy => 1,
                Difficulty.Easy => 3,
                Difficulty.Normal => 5,
                Difficulty.Hard => 7,
                Difficulty.VeryHard => 9,
                _ => 1
            };

            int maxLevel = difficulty switch
            {
                Difficulty.VeryEasy => 3,
                Difficulty.Easy => 5,
                Difficulty.Normal => 7,
                Difficulty.Hard => 9,
                Difficulty.VeryHard => 10,
                _ => 1
            };

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("이곳은 어딜까요?!");
            Console.ResetColor();
            Console.WriteLine($"선택한 사냥터: {difficulty}");

           
            if (dungeonIntroductions.TryGetValue(difficulty, out string introduction))
            {
                Console.WriteLine(introduction);
            }
            else
            {
                Console.WriteLine("사냥터 개업준비중...");
            }

            Console.WriteLine($"\n이곳에서는 몬스터 레벨이 {minLevel} ~ {maxLevel} 사이에서 랜덤으로 결정됩니다.");
            Console.WriteLine("\n1. 전투 시작");
            Console.WriteLine("0. 뒤로가기");

            int input = Utils.GetPlayerInput();
            return input == 1;
        }

        private void StartBattle(List<Monster> monsters)
        {
            Battle battle = new Battle(player, monsters.ToArray());
            battle.StartBattle();
        }

        private List<Monster> GenerateMonsters(Difficulty difficulty)
        {
            List<Monster> filteredMonsters = FilterMonstersByDifficulty(difficulty);
            List<Monster> selectedMonsters = new List<Monster>();

            int monsterCount = random.Next(3, 6); // 3~5마리 랜덤 선택

            for (int i = 0; i < monsterCount; i++)
            {
                int index = random.Next(filteredMonsters.Count);
                Monster monster = filteredMonsters[index].Clone();
                // 난이도에 따른 몬스터 레벨 범위 내에서 랜덤하게 할당
                monster.Level = GetMonsterLevel(difficulty);
                selectedMonsters.Add(monster);
            }

            return selectedMonsters;
        }

        private List<Monster> FilterMonstersByDifficulty(Difficulty difficulty)
        {
            int minId, maxId;
            switch (difficulty)
            {
                case Difficulty.VeryEasy:
                    minId = 10;
                    maxId = 19;
                    break;
                case Difficulty.Easy:
                    minId = 20;
                    maxId = 29;
                    break;
                case Difficulty.Normal:
                    minId = 30;
                    maxId = 39;
                    break;
                case Difficulty.Hard:
                    minId = 40;
                    maxId = 49;
                    break;
                case Difficulty.VeryHard:
                    minId = 50;
                    maxId = 59;
                    break;
                default:
                    minId = 1;
                    maxId = int.MaxValue;
                    break;
            }

            
            var filtered = monsters.Where(mon => mon.Id >= minId && mon.Id <= maxId).ToList();
            return filtered.Count > 0 ? filtered : monsters;
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
                Difficulty.VeryEasy => random.Next(1, 4),    // 1 ~ 3
                Difficulty.Easy => random.Next(3, 6),    // 3 ~ 5
                Difficulty.Normal => random.Next(5, 8),    // 5 ~ 7
                Difficulty.Hard => random.Next(7, 10),   // 7 ~ 9
                Difficulty.VeryHard => random.Next(9, 11),   // 9 ~ 10
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
