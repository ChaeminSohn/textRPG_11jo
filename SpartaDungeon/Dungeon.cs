using System.Linq.Expressions;
using System.Threading;

namespace SpartaDungeon
{
    internal class Dungeon
    {
        private Player player;

        public Dungeon(Player player)
        {
            this.player = player;
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
            BattleResult result = battle.StartBattle();// 파일 받으면 수정 or 다시

            Console.Clear();
            result.DisplayResult(player);

            while (Utils.GetPlayerInput() != 0)
            {
                Console.WriteLine("0번을 눌러 다음으로 진행하세요.");
            }
        }

        private List<Monster> GenerateMonsters(Difficulty difficulty)
        {
            List<Monster> allMonsters = MonsterList.GetAllMonsters();// 파일 받으면 수정 or 다시
            List<Monster> selectedMonsters = new List<Monster>();
            Random rand = new Random();

            int monsterCount = difficulty switch
            {
                Difficulty.Easy => 3,
                Difficulty.Normal => 4,
                Difficulty.Hard => 5,
                _ => 3
            };

            for (int i = 0; i < monsterCount; i++)
            {
                int index = rand.Next(allMonsters.Count);
                selectedMonsters.Add(allMonsters[index].Clone());
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