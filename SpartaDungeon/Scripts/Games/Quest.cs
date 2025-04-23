using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum QuestStatus
    {
        NotAccepted,
        InProgress,
        Completed
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
                Title = "",
                Description = " ",
                Condition = "",
                Reward = "G",
                Status = QuestStatus.NotAccepted
            });

            availableQuests.Add(new Quest
            {
                Title = "",
                Description = "",
                Condition = "",
                Reward = "G",
                Status = QuestStatus.NotAccepted
            });

            availableQuests.Add(new Quest
            {
                Title = "",
                Description = "",
                Condition = "",
                Reward = "G",
                Status = QuestStatus.NotAccepted
            });

            availableQuests.Add(new Quest
            {
                Title = "",
                Description = "",
                Condition = "",
                Reward = "G",
                Status = QuestStatus.NotAccepted
            });

            availableQuests.Add(new Quest
            {
                Title = "",
                Description = "",
                Condition = "",
                Reward = "G",
                Status = QuestStatus.NotAccepted
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
                selected.Status = QuestStatus.InProgress;
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


