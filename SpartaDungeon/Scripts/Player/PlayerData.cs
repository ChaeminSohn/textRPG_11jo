namespace SpartaDungeon
{
    public class PlayerData     //플레이어의 중요한 정보를 따로 저장하는 클래스
    {
        public string Name { get; private set; }    //이름
        public Job Job { get; private set; }    //직업
        public int Level { get; private set; } = 1; //레벨
        public int MaxLevel { get; set; }   //최대 레벨
        public int Experience { get; private set; }   //경험치
        public int[] ExpThresholds { get; set; } //경험치 상한선
        public int BaseFullHP { get; set; }    //기본 체력
        public int CurrentHP { get; set; }  //현재 체력
        public int BaseAttack { get; set; } //기본 공격력
        public int BaseDefense { get; set; }    //기본 방어력
        public float BaseCritChance { get; set; }   //치명타 확률
        public float BaseEvadeChance { get; set; }   //회피 확률
        public int Meso { get; set; }   //메소
        public int FullMP { get; set; }  // 최대 마나
        public int CurrentMP { get; set; }  // 현재 마나

        public PlayerData(string Name, Job job, int level, int maxLevel, int experience, int[] expThresholds,
            int baseFullHP, int currentHP, int baseAttack, int baseDefense, float baseCritChance, float baseEvadeChance, int meso, int fullMP, int currentMP)
        {
            this.Name = Name;
            Job = job;
            Level = level;
            MaxLevel = maxLevel;
            Experience = experience;
            ExpThresholds = expThresholds;
            BaseFullHP = baseFullHP;
            CurrentHP = currentHP;
            BaseAttack = baseAttack;
            BaseDefense = baseDefense;
            BaseCritChance = baseCritChance;
            BaseEvadeChance = baseEvadeChance;
            Meso = meso;
            FullMP = fullMP;
            CurrentMP = currentMP;
        }
    }
}