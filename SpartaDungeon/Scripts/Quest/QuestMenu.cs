namespace SpartaDungeon.Scripts.Quest
{
    internal class QuestMenu
    {
        Player player;
        private List<Quest> myQuests = new List<Quest>();
        private List<Quest> availableQuests = new List<Quest>();

        public QuestMenu(Player player)
        {
            this.player = player;
            foreach (QuestInfo info in QuestDataBase.QuestData)
            {
                Quest quest = new Quest(info);
                (info.IsAccepted ? myQuests : availableQuests).Add(quest);
            }
        }
        public QuestMenu(Player player, List<QuestInfo> questInfos)
        {
            this.player = player;
            foreach (QuestInfo info in questInfos)
            {
                Quest quest = new Quest(info);
                (info.IsAccepted ? myQuests : availableQuests).Add(quest);
            }
            foreach (Quest quest in myQuests)
            {
                quest.Condition.OnDataLoad();
            }
        }

        public List<QuestInfo> GetQuestSaveData()
        {
            List<QuestInfo> questInfos = new List<QuestInfo>();
            foreach (Quest quest in myQuests)
            {
                questInfos.Add(quest.ToInfo());
            }
            foreach (Quest quest in availableQuests)
            {
                questInfos.Add(quest.ToInfo());
            }
            return questInfos;
        }


        // 퀘스트 게시판 열기
        public void OpenQuestBoard()
        {
            foreach (Quest quest in myQuests)
            {
                quest.Condition.UpdateProgress(player);
            }
            while (true)
            {
                Console.Clear();
                ColorFont.Write("<퀘스트 게시판>\n\n", Color.DarkYellow);
                Console.WriteLine("1. 내 퀘스트");
                Console.WriteLine("2. 새 퀘스트");
                ColorFont.Write("\n0. 뒤로가기\n", Color.Magenta);

                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        ShowMyQuests();
                        break;
                    case 2:
                        ShowAvailableQuests();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        // 내 퀘스트 목록 보기
        private void ShowMyQuests()
        {
            while (true)
            {
                Console.Clear();
                ColorFont.Write("<내 퀘스트>\n\n", Color.DarkYellow);

                if (myQuests.Count == 0)
                {
                    Console.WriteLine("받은 퀘스트가 없습니다.");
                    Utils.Pause(false);
                    return;
                }
                else
                {
                    for (int i = 0; i < myQuests.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {myQuests[i].Title}" +
                         $"({(myQuests[i].Condition.CanComplete ? "완료" : "진행중")})");
                    }
                }
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n퀘스트 진행상황을 확인할 수 있습니다.\n");
                int playerInput = Utils.GetPlayerInput();

                if (playerInput > myQuests.Count || playerInput == -1)
                {
                    //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }
                else if (playerInput == 0)
                {
                    return;
                }
                else    //올바른 퀘스트 번호 입력
                {
                    Quest selected = myQuests[playerInput - 1];
                    if (selected.Condition.CanComplete)
                    {   //퀘스트 완료 가능
                        selected.IsCompleted = true;
                        selected.Condition.OnQuestComplete(player);
                        player.ChangeMeso(selected.Reward);     //보상 메소 지급
                        myQuests.Remove(selected);              //퀘스트 목록에서 삭제
                        Console.WriteLine($"[퀘스트 완료] {selected.Title} 퀘스트를 완료했습니다!");
                        Console.WriteLine($"보상으로 {selected.Reward} 메소를 획득했습니다.");
                    }
                    else
                    {
                        myQuests[playerInput - 1].Condition.ShowProgress();
                    }
                    Utils.Pause(false);
                }
            }
        }

        // 퀘스트 목록 보기
        private void ShowAvailableQuests()
        {
            while (true)
            {
                Console.Clear();
                ColorFont.Write("<새 퀘스트>\n\n", Color.DarkYellow);

                for (int i = 0; i < availableQuests.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableQuests[i].Title}");
                }

                Console.WriteLine("0. 뒤로가기");

                int playerInput = Utils.GetPlayerInput();

                if (playerInput > availableQuests.Count || playerInput == -1)
                {
                    //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }
                else if (playerInput == 0)
                {
                    return;
                }
                else    //올바른 퀘스트 번호 입력
                {
                    ShowQuestDetail(playerInput - 1);
                }
            }
        }

        // 퀘스트 상세 보기
        private void ShowQuestDetail(int index)
        {
            while (true)
            {
                Quest selected = availableQuests[index];
                Console.Clear();
                selected.DisplayDetails();

                Console.WriteLine("\n1. 수락");
                Console.WriteLine("2. 거절");

                switch (Utils.GetPlayerInput())
                {
                    case 1:
                        selected.IsAccepted = true;
                        myQuests.Add(selected);
                        availableQuests.RemoveAt(index);
                        selected.Condition.OnQuestExcepted();
                        selected.Condition.UpdateProgress(player);
                        Console.WriteLine("\n퀘스트를 수락했습니다!");
                        Utils.Pause(false);
                        return;
                    case 2:
                        Console.WriteLine("\n퀘스트를 거절했습니다.");
                        Utils.Pause(false);
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        continue;
                }
            }
        }
    }
}