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

        public Skill(string name, string description, int mp)
        {
            Name = name;
            Description = description;
            MP = mp;
        }

        public abstract void Activate(IBattleUnit attacker, IBattleUnit defender, Monster[] monsters);
    }

    // 스킬을 직업 별로 구분
    public abstract class WarriorSkill : Skill
    {
        protected WarriorSkill(string name, string description, int mp)
            : base(name, description, mp) { }
    }

    public abstract class MageSkill : Skill
    {
        protected MageSkill(string name, string description, int mp)
            : base(name, description, mp) { }
    }

    public abstract class ArcherSkill : Skill
    {
        protected ArcherSkill(string name, string description, int mp)
            : base(name, description, mp) { }
    }

    public abstract class MonsterSkill : Skill
    {
        protected MonsterSkill(string name, string description, int mp)
            : base(name, description, mp) { }
    }

}
