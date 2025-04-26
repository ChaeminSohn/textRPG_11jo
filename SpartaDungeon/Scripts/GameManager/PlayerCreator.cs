namespace SpartaDungeon
{
    public static class PlayerCreator
    {
        public static string GetPlayerName()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("메이플 월드에 오신 걸 환영합니다!");
                Console.WriteLine("플레이어 이름을 입력해주세요.");
                Console.Write("\n>>");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 12)
                {
                    Console.WriteLine("이름은 2자 이상 12자 이하여야 합니다.");
                    Utils.Pause(false);
                    continue;
                }

                Console.WriteLine($"입력한 이름은 {input}입니다.\n1. 확인  2. 취소");
                if (Utils.GetPlayerInput() == 1) return input;
            }
        }

        public static Job SelectJob()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("직업을 선택하세요.\n");
                foreach (Job job in Enum.GetValues(typeof(Job)))
                    Console.WriteLine($"{(int)job + 1}. {Utils.JobDisplayNames[job]}");

                int input = Utils.GetPlayerInput();
                if (input >= 1 && input <= 4)
                {
                    Job selected = (Job)(input - 1);
                    Console.WriteLine($"{Utils.JobDisplayNames[selected]}을 선택하시겠습니까?\n1. 확인  2. 취소");
                    if (Utils.GetPlayerInput() == 1) return selected;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                }
            }
        }
    }
}