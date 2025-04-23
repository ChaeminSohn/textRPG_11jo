using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpartaDungeon
{
    public class Monster : IBattleUnit
    {
        private int level = 1;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Level
        {
            get { return level; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value), "레벨은 1 이상이어야 합니다.");
                level = value;
                UpdateStats();    // 레벨이 바뀔 때마다 능력치 재계산
            }
        }
        public int FullHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public bool IsDead { get; set; }
        public float CritChance { get; set; }
        public float EvadeChance { get; set; }
        public int ExpReward { get; set; }  //처치 시 보상 경험치
        public int GoldReward { get; set; }   //처치 시 골드 보상
        public List<DropTableEntry> Drops { get; set; } = new List<DropTableEntry>();  //아이템 드랍 테이블

        // 레벨당 증가량 설정
        private int hpPerLevel = 5;
        private float attackPerLevel = 0.5f;
        private int defensePerLevel = 1;

        [JsonConstructor]
        public Monster(int id, string name, int level, int fullHP, int attack, int defense,
                        float critChance, float evadeChance, List<DropTableEntry> drops)
        {
            Id = id;
            Name = name;
            Level = level;
            FullHP = fullHP;
            CurrentHP = FullHP;
            Attack = attack;
            Defense = defense;
            CritChance = critChance;
            EvadeChance = evadeChance;
            IsDead = false;
            Drops = drops;
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
            IsDead = true;
        }

        public void UpdateStats()
        {
            FullHP += (level - 1) * hpPerLevel;
            CurrentHP = FullHP;
            Attack += (int)((level - 1) * attackPerLevel);
            Defense += (level - 1) * defensePerLevel;
        }

        public Monster Clone()
        {
            // Clone 시 레벨에 맞게 능력치 업데이트가 필요하므로, 새로 복사한 후 능력치 업데이트
            Monster clone = new Monster(Id, Name, Level, FullHP, Attack, Defense, CritChance, EvadeChance, Drops);
            return clone;
        }
    }
}
