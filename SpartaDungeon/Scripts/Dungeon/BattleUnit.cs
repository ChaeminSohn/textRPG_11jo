namespace SpartaDungeon
{
    public abstract class BattleUnit
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
        public float BaseCritChance { get; protected set; }    //기본 치명타 확률
        public float BonusCritChance { get; protected set; }    //추가 치명타 확률
        public float CritChance => BaseCritChance + BonusCritChance;
        public float BaseEvadeChance { get; set; }      //기본 회피 확률
        public float BonusEvadeChance { get; set; }      //추가 회피 확률
        public float EvadeChance => BaseEvadeChance + BonusEvadeChance;      //회피 확률
        public int FullMP { get; protected set; }  //최대 마나
        public int CurrentMP { get; protected set; }      //현재 마나

        public List<Skill> Skills { get; } = new List<Skill>(); // 스킬 목록

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
            if (FullHP < CurrentHP)
                CurrentHP = FullHP;
        }


        public void RecoverMP(int mp)  // MP회복
        {
            CurrentMP += mp;
            if (FullMP < CurrentMP)
                CurrentMP = FullMP;
        }

        public void UseMP(int mp)  // MP사용
        {
            CurrentMP -= mp;
            if (CurrentMP <= 0)
                CurrentMP = 0;
        }


        private void OnDie() //사망
        {
            IsDead = true;
        }

        protected abstract void LevelUpStats(); // 레벨 업 능력치 상승

        public int FinalDamage(BattleUnit defender, float skillDmg) //최종 데미지 계산
        {
            Random _rand = new Random();
            int baseDamage; // 데미지
            int damageVariance = (int)(Attack * 0.1); // 데미지 편차

            bool isCritical = _rand.NextDouble() < CritChance; // 크리티컬 확률
            if (skillDmg == 0) baseDamage = _rand.Next(Attack - damageVariance, Attack + damageVariance + 1); // 기본 공격 데미지
            else // 스킬 데미지 계산
            {
                int dmg = (int)(Attack * skillDmg);
                baseDamage = _rand.Next(dmg - damageVariance, dmg + damageVariance + 1);
            }
            int finalDamage = isCritical ? (int)(baseDamage * 1.5f) : baseDamage;   //최종 데미지

            if (isCritical)
            {
                ColorFont.Write("[치명타]", Color.Yellow);
                Console.Write(" 데미지가 50% 증가했습니다.\n");
            }

            return finalDamage;
        }

        public void AutoAttack(BattleUnit defender) // 기본 공격
        {
            Console.Write($"\n\nLv.{Level} [{Name}] 의 ");
            ColorFont.Write("공격!\n", Color.DarkRed);

            Random rand = new Random();
            if (rand.NextDouble() < defender.EvadeChance)   //회피 판정
            {
                Console.WriteLine($"Lv.{defender.Level} {defender.Name} 이(가) 공격을 회피했습니다!");
                return;
            }

            DamageResult(defender, 0);
        }

        public void DamageResult(BattleUnit defender, float skillDmg) // 데미지 결과
        {
            int previousHP = defender.CurrentHP;
            int dmg = FinalDamage(defender, skillDmg);
            defender.OnDamage(dmg); //데미지 처리

            Console.Write($"Lv.{defender.Level} [{defender.Name}] ");
            Console.Write($"HP {previousHP} -> {(defender.IsDead ? "Dead" : defender.CurrentHP)} ");
            Console.Write($"[데미지 : {dmg}]\n");
        }
    }
}