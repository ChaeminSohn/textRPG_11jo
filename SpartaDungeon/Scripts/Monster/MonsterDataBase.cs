namespace SpartaDungeon
{
    public static class MonsterDataBase
    {
        public static Dictionary<int, Monster> MonsterDict = new Dictionary<int, Monster>(); //몬스터 사전
        public static Dictionary<int, int> MonsterKillCount = new Dictionary<int, int>();   //몬스터 처치 수

        public static void Load(string path)
        {
            if (!ConfigLoader.TryLoad<MonsterConfig>(Path.Combine(path, "monster_config.json"), out var config)) //모든 몬스터 정보 불러오기
            {
                Console.WriteLine("몬스터 데이터 파일이 존재하지 않습니다.");
                Utils.Pause(false);
                return;
            }
            if (File.Exists(Path.Combine(path, "saveData.json")))   //저장 파일이 존재하는 경우
            {   //몬스터 처치 데이터 불러오기 
                if (!ConfigLoader.TryLoad<GameSaveData>(Path.Combine(path, "saveData.json"), out var countConfig))
                {
                    Console.WriteLine("몬스터 처치 수 데이터 불러오기 실패");
                    Utils.Pause(false);
                    return;
                }
                MonsterKillCount = countConfig.MonsterKillData;
            }
            else
            {
                foreach (Monster monster in config.Monster)     //몬스터 등록
                {
                    MonsterKillCount[monster.Id] = 0;
                }
            }
            foreach (Monster monster in config.Monster)     //몬스터 등록
            {
                MonsterDict[monster.Id] = monster;
            }
        }
    }
}