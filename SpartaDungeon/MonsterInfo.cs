using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpartaDungeon
{
    public class MonsterInfo : IBattleUnit
    {
        private int level = 1;
        public int Id { get; set; }
        public string Name{ get; set; }
        public int Level 
        {
            get => level;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value), "레벨은 1 이상이어야 합니다.");
                level = value;
                UpdateStats();    // 레벨이 바뀔 때마다 능력치 재계산
            }
        }
        public int FullHP { get;  set; }
        public int CurrentHP { get;  set; }
        public int Attack { get;  set; }
        public int Defense { get;  set; }

        // 레벨당 증가량 설정
        private int hpPerLevel = 5; 
        private float attackPerLevel = 0.5f;
        private int defensePerLevel = 1;

        public MonsterInfo(int id, string name, int level, int fullHP, int attack, int defense)
        {
            Id = id;
            Name = name;
            Level = level;
            FullHP = fullHP;
            CurrentHP = FullHP;
            Attack = attack; 
            Defense = defense;
        }

        public void OnDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                OnDie();
            }

        }

        public void RecoverHP(int hp) 
        {
            CurrentHP += hp;
            if (FullHP > CurrentHP)
                CurrentHP = FullHP;
        }

        public void OnDie()
        {

        }

        private void UpdateStats()
        {
            FullHP += (level - 1) * hpPerLevel;
            Attack +=(int)((level - 1) * attackPerLevel);
            Defense += (level - 1) * defensePerLevel;
        }
    }
}
