using System.Dynamic;

namespace SpartaDungeon
{
    public interface IQuestCondition
    {
        bool CanComplete { get; }       //완료 가능 여부

        void ShowCondition();
        void OnQuestExcepted();
        void OnDataLoad();      //게임 불러오기 시 실행
        void ShowProgress();    //진행 상황 표시
        void UpdateProgress(Player player);
        void OnQuestComplete(Player player);
    }

    public class KillMonsterCondition : IQuestCondition
    {
        public QuestType Type => QuestType.KillMonster;
        public bool CanComplete { get; set; }

        //몬스터 처치 수는 플레이어가 처치한 총 몬스터 수를 가져와서 계산
        // 퀘스트 수락 시 현재 처치 수를 가져오고, 이를 기준으로 카운트를 계산함
        //게임 불러오기 시 (총 처치 수) - (퀘스트 처치 수)로 다시 계산

        // (몬스터 ID, 개수)
        public Dictionary<int, int> StartKillCount = new Dictionary<int, int>();
        public Dictionary<int, int> TargetCount = new Dictionary<int, int>();
        public Dictionary<int, int> CurrentCount = new Dictionary<int, int>();

        public void ShowCondition()
        {
            foreach (var key in TargetCount.Keys)
            {
                Console.WriteLine($"{MonsterDataBase.MonsterDict[key].Name} : {TargetCount[key]}마리");
            }
        }
        public void OnQuestExcepted()
        {
            foreach (var key in TargetCount.Keys)
            {
                StartKillCount[key] = MonsterDataBase.MonsterKillCount[key];
                CurrentCount[key] = 0;
            }
        }

        public void OnDataLoad()
        {
            foreach (var key in TargetCount.Keys)
            {
                StartKillCount[key] =
                MonsterDataBase.MonsterKillCount[key] - CurrentCount[key];
            }
        }
        public void ShowProgress()
        {
            foreach (var key in TargetCount.Keys)
            {
                Console.WriteLine($"{MonsterDataBase.MonsterDict[key].Name} : " +
                   $"{CurrentCount[key]} / {TargetCount[key]}");
            }
        }
        public void UpdateProgress(Player player)
        {
            bool isFinished = true;
            foreach (var key in TargetCount.Keys)
            {
                CurrentCount[key] =
                    MonsterDataBase.MonsterKillCount[key] - StartKillCount[key];
                if (CurrentCount[key] < TargetCount[key])
                {
                    isFinished = false;
                }
            }
            CanComplete = isFinished;
        }

        public void OnQuestComplete(Player player)
        {

        }
    }
    public class CollectItemCondition : IQuestCondition
    {
        public QuestType Type => QuestType.CollectItem;
        public bool CanComplete { get; set; }

        // (아이템 ID, 개수)
        public Dictionary<int, int> TargetCount = new Dictionary<int, int>();
        public Dictionary<int, int> CurrentCount = new Dictionary<int, int>();


        public void ShowCondition()
        {
            foreach (var key in TargetCount.Keys)
            {
                Console.WriteLine($"{ItemDataBase.ItemInfoDict[key].Name} : {TargetCount[key]}개");
            }
        }
        public void OnQuestExcepted()
        {
            foreach (var key in TargetCount.Keys)
            {
                CurrentCount[key] = 0;
            }
        }
        public void OnDataLoad()
        {

        }
        public void ShowProgress()
        {
            foreach (var key in TargetCount.Keys)
            {
                Console.WriteLine($"\n {ItemDataBase.ItemInfoDict[key].Name} : " +
                   $"{CurrentCount[key]} / {TargetCount[key]}");
            }
        }
        public void UpdateProgress(Player player)
        {
            bool isFinished = true;
            foreach (var key in TargetCount.Keys)
            {
                CurrentCount[key] = player.Inventory.GetItemCount(key);
                if (CurrentCount[key] < TargetCount[key])
                {
                    isFinished = false;
                }
            }
            CanComplete = isFinished;
        }
        public void OnQuestComplete(Player player)
        {   //플레이어 인벤토리에서 필요한 수치만큼 아이템 제거
            foreach (var key in TargetCount.Keys)
            {
                player.Inventory.ReduceItemCount(key, TargetCount[key]);
            }

        }
    }
}