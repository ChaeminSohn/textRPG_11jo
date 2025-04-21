using System.Diagnostics.Tracing;

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
        public int Gold { get; set; }   //골드

        public PlayerData(string Name, Job job, int level, int maxLevel, int experience, int[] expThresholds,
            int baseFullHP, int currentHP, int baseAttack, int baseDefense, int gold)
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
            Gold = gold;
        }
    }


}