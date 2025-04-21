using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal static class Utils //다양한 곳에서 자주 쓰이는 기능을 모아놓은 클래스
    {
        // 실제 출력 너비 기준으로 문자열 오른쪽 패딩
        public static string PadToWidth(string str, int totalWidth)
        {
            if (str == null)
            {
                str = "";   //null 이면 빈 문자열로 처리
            }
            int realLength = str.Sum(c => c > 127 ? 2 : 1); // 한글은 너비 2, 영어는 1로 계산
            int padding = Math.Max(0, totalWidth - realLength);
            return str + new string(' ', padding);
        }

        //플레이어 입력 기능 모음
        // boolean 값은 텍스트의 표시 여부(false면 입력만 받음)
        public static int GetPlayerInput()  //플레이어의 입력을 숫자로 반환
        {
            Console.Write("\n>>");
            ConsoleKeyInfo playerInput = Console.ReadKey(true);
            int selectedIndex = -1;

            //0~9 숫자가 입력된 경우
            if (playerInput.Key >= ConsoleKey.D0 && playerInput.Key <= ConsoleKey.D9)
            {
                selectedIndex = (int)playerInput.Key - (int)ConsoleKey.D0;
            }
            else if (playerInput.Key >= ConsoleKey.NumPad0 && playerInput.Key <= ConsoleKey.NumPad9)
            {
                selectedIndex = (int)playerInput.Key - (int)ConsoleKey.NumPad0;
            }
            return selectedIndex; //-1이 반환될 경우, 잘못된 입력
        }

        public static void Pause(bool withText)  //게임 퍼즈 - 아무 키나 입력하면 진행
        {
            if (withText)
            {
                Console.WriteLine("\n아무 키나 누르면 계속합니다...");
            }
            Console.ReadKey();
        }


        // 열거형 출력 전용 딕셔너리 모음
        public static Dictionary<Job, string> JobDisplayNames = new Dictionary<Job, string>
        {
            {Job.Warrior, "전사"},
            {Job.Mage, "마법사"},
            {Job.Archer, "궁수"}
        };
        public static Dictionary<Stat, string> StatDisplayNames = new Dictionary<Stat, string>
        {
            {Stat.Health, "체력"},
            {Stat.Attack, "공격력"},
            {Stat.Defense, "방어력"}
        };
        public static Dictionary<ItemType, string> ItemTypeDisplayNames = new Dictionary<ItemType, string>
        {
            {ItemType.Equipment, "장비"},
            {ItemType.Usable, "소비"},
            {ItemType.Other, "기타"}
        };

        public static Dictionary<EquipType, string> EquipTypeDisplayNames = new Dictionary<EquipType, string>
        {
            {EquipType.Armor, "방어구"},
            {EquipType.Weapon, "무기"}
        };
    }
}
