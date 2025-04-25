namespace SpartaDungeon
{
    public static class QuestDataBase
    {
        public static List<QuestInfo> QuestData = new List<QuestInfo>();    //퀘스트 모음

        public static void Load(string path)
        {
            if (!ConfigLoader.TryLoad<List<QuestInfo>>(path, out var config))
            {
                Console.WriteLine("퀘스트 데이터 파일이 존재하지 않습니다.");
                Utils.Pause(false);
                return;
            }
            QuestData = config;
        }
    }

    public static class QuestConditionFactory
    {
        public static IQuestCondition Create(QuestInfo info)
        {
            if (info.CurrentCount == null)
            {
                info.CurrentCount = new Dictionary<int, int>();
            }

            switch (info.Type)
            {
                case QuestType.KillMonster:
                    return new KillMonsterCondition
                    {
                        TargetCount = new Dictionary<int, int>(info.TargetCount),
                        CurrentCount = new Dictionary<int, int>(info.CurrentCount),
                    };
                case QuestType.CollectItem:
                    return new CollectItemCondition
                    {
                        TargetCount = new Dictionary<int, int>(info.TargetCount),
                        CurrentCount = new Dictionary<int, int>(info.CurrentCount),
                    };

                default:
                    throw new NotSupportedException($"지원하지 않는 퀘스트 타입: {info.Type}");
            }
        }

        public static QuestInfo ToInfo(IQuestCondition condition)
        {
            if (condition is KillMonsterCondition km)
            {
                return new QuestInfo
                {
                    Type = QuestType.KillMonster,
                    TargetCount = new Dictionary<int, int>(km.TargetCount),
                    CurrentCount = new Dictionary<int, int>(km.CurrentCount)
                };
            }
            else if (condition is CollectItemCondition ci)
            {
                return new QuestInfo
                {
                    Type = QuestType.KillMonster,
                    TargetCount = new Dictionary<int, int>(ci.TargetCount),
                    CurrentCount = new Dictionary<int, int>(ci.CurrentCount)
                };
            }

            throw new NotSupportedException($"직렬화할 수 없는 조건 타입: {condition.GetType()}");
        }
    }
}