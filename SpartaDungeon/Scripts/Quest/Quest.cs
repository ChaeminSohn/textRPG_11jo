using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum QuestType
    {
        KillMonster,
        CollectItem
    }
    internal class Quest
    {
        public string Title;
        public QuestType Type { get; set; }
        public string Description;
        public IQuestCondition Condition { get; set; }
        public int Reward { get; set; }

        public bool IsAccepted { get; set; }  //퀘스트 수락 여부
        public bool IsCompleted { get; set; } //퀘스트 완료 여부
        public Dictionary<int, int> TargetCount { get; set; }    // ID, 개수
        public Dictionary<int, int> CurrentCount { get; set; }
        public Dictionary<int, int> RequiredItems = new Dictionary<int, int>();


        public Quest(QuestInfo info)
        {
            Title = info.Title;
            Type = info.Type;
            Description = info.Description;
            Reward = info.Reward;
            IsAccepted = info.IsAccepted;
            IsCompleted = info.IsCompleted;
            TargetCount = info.TargetCount;
            CurrentCount = info.CurrentCount;
            // 조건 생성
            Condition = QuestConditionFactory.Create(info);
        }
        public QuestInfo ToInfo()
        {
            return new QuestInfo
            {
                Title = Title,
                Description = Description,
                Reward = Reward,
                IsAccepted = IsAccepted,
                IsCompleted = IsCompleted,
                TargetCount = TargetCount,
                CurrentCount = CurrentCount
            };
        }
        public void DisplayDetails()
        {
            //int reward = int.Parse(Reward);  // 예외 처리 가능
            Console.WriteLine($"\n===== 퀘스트 상세 정보 =====");
            Console.WriteLine($"제목     : {Title}");
            Console.WriteLine($"설명     : {Description}");
            Condition.ShowCondition();
            Console.WriteLine($"보상     : {Reward}메소");
        }
        public void TryAutoComplete(Inventory inventory)
        {

            bool allItemsPresent = true;
            foreach (var req in RequiredItems)
            {
                var item = inventory.Others.FirstOrDefault(o => o.ID == req.Key);
                if (item is OtherItem otherItem)
                {
                    if (otherItem.ItemCount < req.Value)
                    {
                        allItemsPresent = false;
                        break;
                    }
                }
                else
                {
                    allItemsPresent = false;
                    break;
                }
            }
            if (allItemsPresent)
            {
                // 아이템 제거
                foreach (var req in RequiredItems)
                {
                    var item = inventory.Others.FirstOrDefault(o => o.ID == req.Key);
                    if (item is OtherItem otherItem)
                    {
                        otherItem.ChangeItemCount(-req.Value);
                        if (otherItem.ItemCount <= 0)
                        {
                            inventory.RemoveItem(otherItem);
                        }
                    }
                }
                Console.WriteLine($"[퀘스트 완료] {Title} 퀘스트를 완료했습니다!");
                Console.WriteLine($"보상으로 {Reward} 메소를 획득했습니다.");
                Utils.Pause(false);
            }
        }
    }
}