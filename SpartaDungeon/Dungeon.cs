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
                Console.WriteLine("1. 쉬움");
                Console.WriteLine("2. 보통");
                Console.WriteLine("3. 어려움");
                Console.WriteLine("0. 나가기");

                int input = Utils.GetPlayerInput();

                switch (input)
                {
                    case 0:
                        return;
                    case 1:
                        StartBattle(GenerateMonsters(Difficulty.Easy));
                        return;
                    case 2:
                        StartBattle(GenerateMonsters(Difficulty.Normal));
                        return;
                    case 3:
                        StartBattle(GenerateMonsters(Difficulty.Hard));
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        private void StartBattle(List<Monster> monsters)
        {
            Battle battle = new Battle(player, monsters.ToArray());
            battle.StartBattle();

            Console.Clear();

            while (Utils.GetPlayerInput() != 0)
            {
                Console.WriteLine("0번을 눌러 다음으로 진행하세요.");
            }
        }

        private List<Monster> GenerateMonsters(Difficulty difficulty)
        {
            List<Monster> allMonsters = monsters;
            List<Monster> selectedMonsters = new List<Monster>();
            Random rand = new Random();

            int monsterCount = rand.Next(3, 5); // 3~5마리 랜덤 선택

            int monsterLevel = difficulty switch
            {
                Difficulty.Easy => 1,
                Difficulty.Normal => 3,
                Difficulty.Hard => 5,
                _ => 1
            };

            for (int i = 0; i < monsterCount; i++)
            {
                int index = rand.Next(allMonsters.Count);
                Monster monster = allMonsters[index].Clone(); // 몬스터 복제
                monster.Level = monsterLevel; // 난이도에 맞는 레벨 설정
                selectedMonsters.Add(monster);
            }

            return selectedMonsters;
        }
    }
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}