using System.Linq.Expressions;

namespace SpartaDungeon
{
    internal class Dungeon
    {
        //던전은 총 3단계로 구성. 쉬움 : 0, 보통 : 1, 어려움 : 2

        //권장 방어력
        public int[] DefenseLevel { get; private set; } = { 5, 10, 20 };
        //보상 골드
        public int[] GoldReward { get; private set; } = { 1000, 1700, 2500 };
        //보상 경험치
        public int[] ExpAward { get; private set; } = { 1, 2, 3 };
        private static Random rand = new Random();

        public void DungeonAction(Player player)    // 4 : 던전 입장 액션
        {
            while (true)
            {
                int health = player.CurrentHP;
                int gold = player.Gold;
                Console.Clear();
                Console.WriteLine("<던전 입장>");
                Console.WriteLine("도전할 던전의 난이도를 선택하세요.");
                Console.WriteLine($"\n1. 쉬운 던전     | 방어력 {DefenseLevel[0]} 이상 권장");
                Console.WriteLine($"2. 보통 던전     | 방어력 {DefenseLevel[1]} 이상 권장");
                Console.WriteLine($"3. 어려운 던전   | 방어력 {DefenseLevel[2]} 이상 권장");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        if (EnterDungeon(player, 1))
                        {   //던전 성공 시
                            Console.Clear();
                            Console.WriteLine("<던전 클리어>");
                            Console.WriteLine("축하합니다!");
                            Console.WriteLine("쉬운 던전을 클리어 하셨습니다.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Console.WriteLine($"Gold {gold} -> {player.Gold}");
                            Utils.Pause(true);
                            player.GetEXP(ExpAward[0]);
                            break;
                        }
                        else if (player.CurrentHP > 0)  //던전 실패 시 + 플레이어 사망 상태 아님
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Utils.Pause(true);
                            return;
                        }
                        break;

                    case 2:
                        if (EnterDungeon(player, 2))
                        {   //던전 성공 시
                            Console.Clear();
                            Console.WriteLine("<던전 클리어>");
                            Console.WriteLine("축하합니다!");
                            Console.WriteLine("보통 던전을 클리어 하셨습니다.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Console.WriteLine($"Gold {gold} -> {player.Gold}");
                            Utils.Pause(true);
                            player.GetEXP(ExpAward[1]);
                            break;
                        }
                        else if (player.CurrentHP > 0)  //던전 실패 시 + 플레이어 사망 상태 아님
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Utils.Pause(true);
                            return;
                        }
                        break;

                    case 3:
                        if (EnterDungeon(player, 3))
                        {   //던전 성공 시
                            Console.Clear();
                            Console.WriteLine("<던전 클리어>");
                            Console.WriteLine("축하합니다!");
                            Console.WriteLine("어려운 던전을 클리어 하셨습니다.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Console.WriteLine($"Gold {gold} -> {player.Gold}");
                            Utils.Pause(true);
                            player.GetEXP(ExpAward[2]);
                            break;
                        }
                        else if (player.CurrentHP > 0)  //던전 실패 시 + 플레이어 사망 상태 아님
                        {
                            Console.Clear();
                            Console.WriteLine("<던전 실패>");
                            Console.WriteLine("던전에서 패배했습니다...");
                            Console.WriteLine("방어구 레벨을 올리고 다시 시도해보세요.");
                            Console.WriteLine("\n[탐험 결과]");
                            Console.WriteLine($"체력 {health} -> {player.CurrentHP}");
                            Utils.Pause(true);
                            return;
                        }
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
                if (player.CurrentHP <= 0)
                {
                    return;
                }
            }

        }
        public bool EnterDungeon(Player player, int level)  //던전 입장
        {   //던전 성공시 true, 실패시 false 반환
            rand = new Random();
            level--;
            int bonusDamage = player.Defense - DefenseLevel[level];
            if (player.Defense < DefenseLevel[level])   //플레이어의 방어력이 권장 방어력보다 낮을 경우
            {
                if (rand.NextDouble() <= 0.4)  //40프로 확률로 던전 실패
                {
                    //플레이어 체력 절반 감소
                    player.OnDamage(player.FullHP / 2);
                    return false;
                }
            }
            //던전 성공
            //체력 소모 : 내 방어력 - 권장 방어력만큼 랜덤 값에 설정
            int damage = rand.Next(20 - bonusDamage, 35 - bonusDamage);
            player.OnDamage(damage);
            if (player.CurrentHP <= 0)  //성공은 했지만 체력이 0이 된 경우
            {
                return false;
            }
            //공격력 ~ 공격력 * 2 의 % 만큼 추가 보상 획득 가능
            float bonusGold = rand.Next(player.Attack, player.Attack * 2) / 100f + 1f;
            player.ChangeGold((int)(GoldReward[level] * bonusGold));
            return true;
        }
    }
}