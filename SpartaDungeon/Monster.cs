using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpartaDungeon
{
    public struct Monster : IBattleUnit
    {
        private string name;
        private int level;
        private int fullHP;
        private int currentHP;
        private int attack;
        private int defense;
        private bool isDead;
        private float critChance;
        private float evadeChance;

        public string Name { get { return name; } }
        public int Level { get { return level; } }
        public int FullHP { get { return FullHP; } }
        public int CurrentHP { get { return currentHP; } }
        public int Attack { get { return attack; } }
        public int Defense { get { return defense; } }
        public bool IsDead { get { return isDead; } }
        public float CritChance { get { return critChance; } }
        public float EvadeChance { get { return evadeChance; } }

        public Monster(string name, int level, int fullHP, int currentHP, int attack, int defense,
                        float critChance, float evadeChance)
        {
            this.name = name;
            this.level = level;
            this.fullHP = fullHP;
            this.currentHP = currentHP;
            this.attack = attack;
            this.defense = defense;
            this.critChance = critChance;
            this.evadeChance = evadeChance;
            isDead = false;
        }

        public void OnDamage(int damage)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                OnDie();
            }
        }

        public void RecoverHP(int hp)
        {
            currentHP += hp;
            if (fullHP > currentHP)
                currentHP = fullHP;
        }
        public void OnDie()
        {
            isDead = true;
        }

    }
}
