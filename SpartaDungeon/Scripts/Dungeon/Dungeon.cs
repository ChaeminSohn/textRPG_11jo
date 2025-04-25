using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading;


namespace SpartaDungeon
{
    internal class Dungeon
    {
        private Player player;
        private static Random random;
        //private List<Monster> monsters;


        private readonly Dictionary<Difficulty, string> dungeonIntroductions = new Dictionary<Difficulty, string>
        {
            { Difficulty.VeryEasy, "버섯...좋아하세요?!" },
            { Difficulty.Easy,     "개발하다보니 슬라임을 만들고 있는 건에 대하여..." },
            { Difficulty.Normal,   "문어와 유령의 공통점은?? '두발'로 설 수 없습니다. 탈모죠..." },
            { Difficulty.Hard,     "돼지와 나무...꾼?" },
            { Difficulty.VeryHard, "템은 다 맞추고 오시는거 맞죠? 못 잡으실거에요ㅎㅎ" }
        };


        private readonly Dictionary<Difficulty, string> dungeonDecorations = new Dictionary<Difficulty, string>
        {
            { Difficulty.VeryEasy,
@"  +---------------------------+
  |         HENESIS           |
  |  The Beginner's Refuge    |
  +---------------------------+" },
            { Difficulty.Easy,
@"  /===========================\
  |          ELYNIA           |
  |   Realm of the Slime      |
  \===========================/" },
            { Difficulty.Normal,
@"  .---------------------------.
  |       CONNINGCITY         |
  |    Town of Intrigues      |
  '---------------------------'" },
            { Difficulty.Hard,
@"  #############################
  #          PERRION          #
  #  The Warrior's Fortress   #
  #############################" },
            { Difficulty.VeryHard,
@"  *****************************
  *        SLEEPYWOOD         *
  *   Land of Eternal Dreams  *
  *****************************" }
        };


        private readonly string hiddenBossRoomDecoration =
@"  _________________________________
 /                                 \
|      ELNAS (ABANDONED MINE)       |
|   Heart of the Ruined Mine        |
 \_________________________________/";


        private Dictionary<Difficulty, bool> stageCleared = new Dictionary<Difficulty, bool>
        {
            { Difficulty.VeryEasy, false },
            { Difficulty.Easy,     false },
            { Difficulty.Normal,   false },
            { Difficulty.Hard,     false },
            { Difficulty.VeryHard, false }
        };


        private readonly Dictionary<Difficulty, string> bossDialogues = new Dictionary<Difficulty, string>
        {
            { Difficulty.VeryEasy, "머쉬맘: ㅂ...ㅓ....ㅅ..ㅓ..ㅅ!!!!!" },
            { Difficulty.Easy,     "킹슬라임: 다시 태어나보니 내가 킹슬라임?!" },
            { Difficulty.Normal,   "블루 머쉬맘: ㅂ...ㅡ..ㄹ..ㄹ..ㅜ.....버섯!!!" },
            { Difficulty.Hard,     "주니어 발록: 난 도대체 무슨 말을 해야하지???" },
            { Difficulty.VeryHard, "좀비 머쉬맘: 안녕하세요 좀비입니다. 버섯이죠!" }
        };



        private readonly string hiddenBossDialogue = "원석의 힘으로 자쿰이 소환됩니다.";




        public Dungeon(Player player)
        {
            this.player = player;
            //foreach()
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

                if (stageCleared.Values.All(cleared => cleared))
                {
                    Console.WriteLine("6. 엘나스(폐광)");
                }
                Console.WriteLine("0. 나가기");

                int input = Utils.GetPlayerInput();



                if (input == 6 && stageCleared.Values.All(x => x))
                {
                    StartHiddenBossBattle();
                    return;
                }

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

                bool exitDungeon = false;
                while (!exitDungeon)
                {
                    int option = ShowDungeonIntro(selectedDifficulty, stageCleared[selectedDifficulty]);
                    if (option == 0)
                    {
                        exitDungeon = true;
                    }
                    else if (option == 1)
                    {
                        StartNormalBattle(selectedDifficulty);
                        if (player.IsDead) return;
                        stageCleared[selectedDifficulty] = true;
                    }
                    else if (option == 2 && stageCleared[selectedDifficulty])
                    {
                        StartBossBattle(selectedDifficulty);
                        exitDungeon = true;
                    }
                }
                return;
            }
        }

        private int ShowDungeonIntro(Difficulty difficulty, bool cleared)
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

            // 던전 꾸밈 출력
            if (dungeonDecorations.TryGetValue(difficulty, out string? decoration))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(decoration);
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("이곳은 어딜까요?!");
            Console.ResetColor();
            Console.WriteLine($"선택한 사냥터: {difficulty}");

            if (dungeonIntroductions.TryGetValue(difficulty, out string? introduction))
                Console.WriteLine(introduction);
            else
                Console.WriteLine("사냥터 개업준비중...");

            Console.WriteLine($"\n이곳에서는 몬스터 레벨이 {minLevel} ~ {maxLevel} 사이에서 랜덤으로 결정됩니다.");
            if (cleared)
            {
                Console.WriteLine("\n옵션: 1. 일반 전투 재도전    2. 숨겨진 보스방 입장    0. 뒤로가기");
            }
            else
            {
                Console.WriteLine("\n옵션: 1. 전투 시작    0. 뒤로가기");
            }
            int input = Utils.GetPlayerInput();
            return input;
        }

        private void StartNormalBattle(Difficulty difficulty)
        {
            List<Monster> normalMonsters = GenerateNormalMonsters(difficulty);
            Battle battle = new Battle(player, normalMonsters.ToArray());
            battle.StartBattle();
        }


        private void StartBossBattle(Difficulty difficulty)
        {
            Monster boss = GenerateBoss(difficulty);
            if (bossDialogues.TryGetValue(difficulty, out string? dialogue))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(dialogue);
                Console.ResetColor();
                Utils.Pause(false);
            }
            Battle battle = new Battle(player, new Monster[] { boss });
            battle.StartBattle();
        }


        private void StartHiddenBossBattle()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(hiddenBossRoomDecoration);
            Console.ResetColor();


            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(hiddenBossDialogue);
            Console.ResetColor();
            Utils.Pause(false);

            Monster hiddenBoss = GenerateHiddenBoss();
            Battle battle = new Battle(player, new Monster[] { hiddenBoss });
            battle.StartBattle();
        }


        private List<Monster> GenerateNormalMonsters(Difficulty difficulty)
        {
            List<Monster> filteredMonsters = FilterMonstersByDifficulty(difficulty);
            List<Monster> selectedMonsters = new List<Monster>();
            int monsterCount = random.Next(3, 6);
            for (int i = 0; i < monsterCount; i++)
            {
                int index = random.Next(filteredMonsters.Count);
                Monster monster = filteredMonsters[index].Clone();
                monster.Level = GetMonsterLevel(difficulty);
                selectedMonsters.Add(monster);
            }
            return selectedMonsters;
        }


        private Monster? GenerateBoss(Difficulty difficulty)
        {
            int bossId = GetBossId(difficulty);
            Monster? boss = MonsterDataBase.MonsterDict[bossId];
            if (boss == null)
            {
                Console.WriteLine($"[경고] 보스는 휴가중");
                return null;
            }
            boss = boss.Clone();
            boss.Level = GetBossLevel(difficulty);
            return boss;
        }


        private Monster? GenerateHiddenBoss()
        {
            int hiddenBossId = 999;
            Monster? boss = MonsterDataBase.MonsterDict[hiddenBossId];
            if (boss == null)
            {
                Console.WriteLine($"[경고] 보스는 휴가중");
                return null;
            }
            boss = boss.Clone();

            boss.Level = random.Next(20, 26);
            return boss;
        }


        private List<Monster> FilterMonstersByDifficulty(Difficulty difficulty)
        {
            int minId, maxId;
            switch (difficulty)
            {
                case Difficulty.VeryEasy:
                    minId = 10; maxId = 19;
                    break;
                case Difficulty.Easy:
                    minId = 20; maxId = 29;
                    break;
                case Difficulty.Normal:
                    minId = 30; maxId = 39;
                    break;
                case Difficulty.Hard:
                    minId = 40; maxId = 49;
                    break;
                case Difficulty.VeryHard:
                    minId = 50; maxId = 59;
                    break;
                default:
                    minId = 1; maxId = int.MaxValue;
                    break;
            }
            var filtered = MonsterDataBase.MonsterDict
               .Where(pair => pair.Key >= minId && pair.Key <= maxId)
               .Select(pair => pair.Value)
               .ToList();

            return filtered.Count > 0 ? filtered : MonsterDataBase.MonsterDict.Values.ToList();
        }


        private int GetBossId(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.VeryEasy => 101,
                Difficulty.Easy => 102,
                Difficulty.Normal => 103,
                Difficulty.Hard => 104,
                Difficulty.VeryHard => 105,
                _ => 101
            };
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
                Difficulty.Easy => random.Next(3, 6),         // 3 ~ 5
                Difficulty.Normal => random.Next(5, 8),       // 5 ~ 7
                Difficulty.Hard => random.Next(7, 10),         // 7 ~ 9
                Difficulty.VeryHard => random.Next(9, 11),      // 9 ~ 10
                _ => 1
            };
        }

        private int GetBossLevel(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.VeryEasy => random.Next(3, 5),     // 예: 3 ~ 4
                Difficulty.Easy => random.Next(5, 7),          // 예: 5 ~ 6
                Difficulty.Normal => random.Next(7, 9),        // 예: 7 ~ 8
                Difficulty.Hard => random.Next(9, 11),         // 예: 9 ~ 10
                Difficulty.VeryHard => random.Next(11, 13),      // 예: 11 ~ 12
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