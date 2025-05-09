﻿namespace SpartaDungeon
{
    public enum QuestType   //퀘스트 종류
    {
        KillMonster,      //몬스터 처치 
        CollectItem     //아이템 수집 
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
        public Dictionary<int, int> TargetCount { get; set; } = new Dictionary<int, int>();  // ID, 개수
        public Dictionary<int, int> CurrentCount { get; set; } = new Dictionary<int, int>();
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
                TargetCount = Condition.TargetCount,
                CurrentCount = Condition.CurrentCount
            };
        }
        public void DisplayDetails()
        {
            Console.WriteLine($"\n===== 퀘스트 상세 정보 =====\n");
            Console.WriteLine($"제목     : {Title}");
            Console.WriteLine($"설명     : {Description}");
            Condition.ShowCondition();
            Console.WriteLine($"보상     : {Reward}메소");
        }
    }
}