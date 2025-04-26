namespace SpartaDungeon
{
    [Serializable]
    public struct QuestInfo
    {
        public string Title { get; set; }   //제목
        public string Description { get; set; } //설명
        public int Reward { get; set; }     //보상(메소)
        public bool IsAccepted { get; set; }   //수락 여부
        public bool IsCompleted { get; set; }   //완료 여부  
        public QuestType Type { get; set; }  //퀘스트 타입
        public Dictionary<int, int> TargetCount { get; set; }    // ID, 개수
        public Dictionary<int, int> CurrentCount { get; set; }
    }
}