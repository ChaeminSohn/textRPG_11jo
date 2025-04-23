using System.Dynamic;

namespace SpartaDungeon
{
    public abstract class IBattleUnit
    {
        protected int level = 1;
        public string Name { get; set; }    //이름
        public int Level //레벨
        {
            get { return level; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value), "레벨은 1 이상이어야 합니다.");
                level = value;
                LevelUpStats();    // 레벨이 바뀔 때마다 능력치 재계산
            }
        }
        public int BaseFullHP { get; protected set; }     //기본 최대 체력
        public int BonusFullHP { get; protected set; }    //추가 최대 체력
        public int FullHP => BaseFullHP + BonusFullHP;  //최대 체력
        public int CurrentHP { get; set; }       //현재 체력
        public int BaseAttack { get; protected set; }     //기본 공격력
        public int BonusAttack { get; protected set; }    //추가 공격력
        public int Attack => BaseAttack + BonusAttack;  //공력력
        public int BaseDefense { get; protected set; }    //기본 방어력
        public int BonusDefense { get; protected set; }   //추가 방어력
        public int Defense => BaseDefense + BonusDefense;   //방어력
        public bool IsDead { get; set; }      //사망 여부
        public float CritChance { get; set; }    //치명타 확률
        public float EvadeChance { get; set; }      //회피 확률
        public int FullMP { get; protected set; }  //최대 마나
        public int CurrentMP { get; protected set; }      //현재 마나


        public void OnDamage(int damage) //피격
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                OnDie();
            }
        }

        public void RecoverHP(int hp)  //회복
        {
            CurrentHP += hp;
            if (FullHP > CurrentHP)
                CurrentHP = FullHP;
        }

        private void OnDie() //사망
        {
            IsDead = true;
        }

        protected abstract void LevelUpStats(); // 레벨 업 능력치 상승
    }
}