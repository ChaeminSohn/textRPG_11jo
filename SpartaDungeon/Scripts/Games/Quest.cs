using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum QuestStatus
    {
        수락되지않음,
        진행중,
        완료됨
    }

    internal class Quest
    {
        public string Title;
        public string Description;
        public string Condition;
        public string Reward;
        public QuestStatus Status;

        public void DisplayDetails()
        {
            Console.WriteLine($"\n===== 퀘스트 상세 정보 =====");
            Console.WriteLine($"제목     : {Title}");
            Console.WriteLine($"설명     : {Description}");
            Console.WriteLine($"조건     : {Condition}");
            Console.WriteLine($"보상     : {Reward}");
            Console.WriteLine($"상태     : {Status}");
        }
    }

    internal class QuestMenu
    {
        private List<Quest> myQuests = new List<Quest>();
        private List<Quest> availableQuests = new List<Quest>();



        public void InitQuests()
        {
            availableQuests.Add(new Quest
            {
                Title = "간단하고 맛있는 옥토퍼스다리구이",
                Description = "옥토퍼스다리구이 요리법 1. 재료를 구한다. 2. 석쇠로 구우면 끝! ",
                Condition = "컨닝시티 옥토퍼스 사냥터로 가 옥토퍼스다리를 구해보자",
                Reward = "20G",
                Status = QuestStatus.수락되지않음
            });

            availableQuests.Add(new Quest
            {
                Title = "모자가 필요해!",
                Description = "일곱 난쟁이 버섯과 머쉬맘 연극을 하는데 하필 주황색 버섯 모자가 없어요 도와주세요!",
                Condition = "헤네시스 사낭터로 가 주황색버섯을 잡아 모자를 얻자!",
                Reward = "15G",
                Status = QuestStatus.수락되지않음
            });

            availableQuests.Add(new Quest
            {
                Title = "알록달록 달팽이 껍질을 모아봐요!",
                Description = "달팽이 껍질을 모을거예요! 어디에 쓸냐고요? 후후후 그건 비밀~!",
                Condition = "페리온 사낭터로 가 빨간 달팽이 1, 파란달팽이 1 를 잡아 아이템을 얻어보자",
                Reward = "5G",
                Status = QuestStatus.수락되지않음
            });

            availableQuests.Add(new Quest
            {
                Title = "커즈아이 꼬리구이?",
                Description = "옥토퍼스다리구이처럼 커즈아이 꼬리도 맛있을것같아요! 그러니 꼬리 좀 구해주세요",
                Condition = "슬리피우드 사냥터에서 커즈아이 꼬리를 구해보자",
                Reward = "10G",
                Status = QuestStatus.수락되지않음
            });

            availableQuests.Add(new Quest
            {
                Title = "현상수배 킹 슬라임",
                Description = "킹 슬라임을 잡아줘",
                Condition = "엘리니아 던전 보스몹을 잡자",
                Reward = "200G",
                Status = QuestStatus.수락되지않음
            });
        }

        public void OpenQuestBoard()
        {
            Console.Clear();
            Console.WriteLine("=== 퀘스트 게시판 ===");
            Console.WriteLine("1. 내 퀘스트");
            Console.WriteLine("2. 새 퀘스트");
            Console.WriteLine("0. 뒤로가기");

            string input = Console.ReadLine();

            if (input == "1")
            {
                ShowMyQuests();
            }
            else if (input == "2")
            {
                ShowAvailableQuests();
            }
        }

        private void ShowMyQuests()
        {
            Console.Clear();
            Console.WriteLine("=== 내 퀘스트 목록 ===");

            if (myQuests.Count == 0)
            {
                Console.WriteLine("받은 퀘스트가 없습니다.");
            }
            else
            {
                for (int i = 0; i < myQuests.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {myQuests[i].Title} ({myQuests[i].Status})");
                }
            }

            Console.WriteLine("\n아무 키나 눌러 돌아가기...");
            Console.ReadKey();
        }

        private bool questsInitialized = false; // 퀘스트 초기화 여부 플래그

        private void ShowAvailableQuests()
        {
            Console.Clear();

            // 퀘스트 초기화가 아직 안 된 경우 한 번만 실행
            if (!questsInitialized)
            {
                InitQuests();
                questsInitialized = true;
            }

            Console.WriteLine("=== 새 퀘스트 목록 ===");

            for (int i = 0; i < availableQuests.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableQuests[i].Title}");
            }

            Console.WriteLine("0. 뒤로가기");
            Console.Write("선택 > ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int index) && index > 0 && index <= availableQuests.Count)
            {
                ShowQuestDetail(index - 1);
            }
        }

        private void ShowQuestDetail(int index)
        {
            Quest selected = availableQuests[index];
            Console.Clear();
            selected.DisplayDetails();

            Console.WriteLine("\n1. 수락");
            Console.WriteLine("2. 거절");

            string input = Console.ReadLine();
            if (input == "1")
            {
                selected.Status = QuestStatus.진행중;
                myQuests.Add(selected);
                availableQuests.RemoveAt(index);
                Console.WriteLine("\n퀘스트를 수락했습니다!");
            }
            else
            {
                Console.WriteLine("\n퀘스트를 거절했습니다.");
            }

            Console.WriteLine("아무 키나 눌러 돌아가기...");
            Console.ReadKey();
        }
    }

}


