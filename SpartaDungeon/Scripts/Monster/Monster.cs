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
        public int Id { get; set; }
        public int ExpReward { get; set; }  //처치 시 보상 경험치
        public int MesoReward { get; set; }   //처치 시 메소 보상
        public List<DropTableEntry> Drops { get; set; } = new List<DropTableEntry>();  //아이템 드랍 테이블

        // 레벨당 증가량 설정
        private int hpPerLevel = 1;
        private float attackPerLevel = 0.5f;
        private int defensePerLevel = 1;

        [JsonConstructor]
        public Monster(int id, string name, int level, int fullHP, int attack, int defense, int expReward, int mesoReward,
                        float critChance, float evadeChance, List<DropTableEntry> drops)
        {
            Id = id;
            Name = name;
            Level = level;
            BaseFullHP = fullHP;
            CurrentHP = FullHP;
            BaseAttack = attack;
            BaseDefense = defense;
            BaseAttack = attack;
            BaseDefense = defense;
            ExpReward = expReward;
            MesoReward = mesoReward;
            CritChance = critChance;
            EvadeChance = evadeChance;
            IsDead = false;
            Drops = drops;
        }

        protected override void LevelUpStats()
        {
            BaseFullHP += (level - 1) * hpPerLevel;
            CurrentHP = FullHP;
            BaseAttack += (int)((level - 1) * attackPerLevel);
            BaseDefense += (level - 1) * defensePerLevel;
        }

        public Monster Clone()
        {
            // Clone 시 레벨에 맞게 능력치 업데이트가 필요하므로, 새로 복사한 후 능력치 업데이트
            Monster clone = new Monster(Id, Name, Level, FullHP, Attack, Defense, ExpReward, MesoReward,
            CritChance, EvadeChance, Drops);
            return clone;
        }
    }
}
