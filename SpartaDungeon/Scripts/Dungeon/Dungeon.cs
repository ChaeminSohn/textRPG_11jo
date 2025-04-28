using NAudio.Wave;

namespace SpartaDungeon
{

    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;


        public override long Length => long.MaxValue;

        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {

                    sourceStream.Position = 0;
                    bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }

    internal class Dungeon
    {
        private Player player;
        private static Random random;
        private List<Monster> monsters;

        private readonly Dictionary<Difficulty, string> dungeonIntroductions = new Dictionary<Difficulty, string>
        {
            { Difficulty.VeryEasy, "초보자들의 성지, 헤네시스에 오신 것을 환영합니다." },
            { Difficulty.Easy,     "메이플월드에서 가장 높은 숲, 엘리니아에 오신 것을 환영합니다." },
            { Difficulty.Normal,   "가장 비열한 자들의 도시, 커닝시티에 오신 것을 환영합니다." },
            { Difficulty.Hard,     "메마른 땅 위에 세워진 전사들의 마을, 페이온에 오신 것을 환영합니다." },
            { Difficulty.VeryHard, "끝 없는 꿈의 숲, 슬리피우드에 오신 것을 환영합니다." }
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
            { Difficulty.VeryEasy, "어디선가 커다란 버섯이 나타났습니다." },
            { Difficulty.Easy,     "뿡야!!!!" },
            { Difficulty.Normal,   "어디선가 커다란 파란 버섯이 나타났습니다." },
            { Difficulty.Hard,     "그르렁거리는 소리가 들립니다. 사악한 존재가 모습을 드러냈습니다." },
            { Difficulty.VeryHard, "어디선가 음산한 기운을 풍기는 커다란 버섯이 나타났습니다." }
        };

        private readonly string hiddenBossDialogue = "원석의 힘으로 자쿰이 소환됩니다.";


        // NAudio 관련 필드와 난이도별 BGM 파일 경로
        private readonly Dictionary<Difficulty, string> dungeonBgms = new Dictionary<Difficulty, string>
        {
            { Difficulty.VeryEasy, "bgm_veryeasy.mp3" },
            { Difficulty.Easy,     "bgm_easy.mp3" },
            { Difficulty.Normal,   "bgm_normal.mp3" },
            { Difficulty.Hard,     "bgm_hard.mp3" },
            { Difficulty.VeryHard, "bgm_veryhard.mp3" }
        };

        public Dungeon(Player player)
        {
            this.player = player;

            random = new Random();
        }

        public void Enter()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("<사냥터 입장>");
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
                ColorFont.Write("\n0. 나가기\n", Color.Magenta);

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
                SoundManager.StopBgm();
                return;
            }
        }

        private int ShowDungeonIntro(Difficulty difficulty, bool cleared)
        {
            // 해당 난이도의 BGM을 재생합니다.
            SoundManager.PlayBgm(dungeonBgms[difficulty]);
            //PlayBgm(difficulty);

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

            if (dungeonDecorations.TryGetValue(difficulty, out string? decoration))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(decoration);
                Console.ResetColor();
            }

            //Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine("이곳은 어딜까요?!");
            //Console.ResetColor();
            //Console.WriteLine($"선택한 사냥터: {difficulty}\n");

            if (dungeonIntroductions.TryGetValue(difficulty, out string? introduction))
                ColorFont.Write(introduction, Color.Cyan);
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

                boss = monsters.First();

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

                boss = monsters.First();

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
                Difficulty.VeryEasy => random.Next(1, 4),
                Difficulty.Easy => random.Next(3, 6),
                Difficulty.Normal => random.Next(5, 8),
                Difficulty.Hard => random.Next(7, 10),
                Difficulty.VeryHard => random.Next(9, 11),
                _ => 1
            };
        }

        private int GetBossLevel(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.VeryEasy => random.Next(3, 5),
                Difficulty.Easy => random.Next(5, 7),
                Difficulty.Normal => random.Next(7, 9),
                Difficulty.Hard => random.Next(9, 11),
                Difficulty.VeryHard => random.Next(11, 13),
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