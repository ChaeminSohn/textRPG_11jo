using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpartaDungeon
{
    public abstract class Skill
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int MP { get; protected set; }

        public float Damage { get; private set; }

        public Skill(string name, string description, int mp, float damage)
        {
            Name = name;
            Description = description;
            MP = mp;
            Damage = damage;
        }
        //public override void Activate(Player user, Monster target)
    }

    // 스킬을 직업 별로 구분
    public abstract class WarriorSkill : Skill
    {
        protected WarriorSkill(string name, string description, int mp, float damage)
            : base(name, description, mp, damage) { }
    }

    public abstract class MageSkill : Skill
    {
        protected MageSkill(string name, string description, int mp, float damage)
            : base(name, description, mp, damage) { }
    }

    public abstract class ArcherSkill : Skill
    {
        protected ArcherSkill(string name, string description, int mp, float damage)
            : base(name, description, mp, damage) { }
    }

}
