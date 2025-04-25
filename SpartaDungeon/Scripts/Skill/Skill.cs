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

        protected void RandomTargetAttack(IBattleUnit attacker, Monster[] monsters, int num, float dmg) // 랜덤 몬스터를 num번 공격
        {
            int dead = 0;
            int target;
            bool isTarget;
            Random random = new Random();

            for (int i = 0; i < num; i++) // 랜덤 적에게 4번 60% 데미지
            {
                do
                {
                    target = random.Next(0, monsters.Length); // 랜덤 타겟 결정
                    isTarget = monsters[target].IsDead;
                    if (isTarget == false)
                    {
                        attacker.DamageResult(monsters[target], dmg);
                    }
                    else // 모든 몬스터가 죽었는지 확인
                    {
                        foreach (Monster monster in monsters)
                        {
                            if (monster.IsDead)
                            {
                                dead++;
                            }
                        }
                    }

                    if (dead == monsters.Length) break; // 모든 몬스터가 죽었으면 공격 중지
                    else dead = 0;

                } while (isTarget == true);
            }
        }
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

    public abstract class AssassinSkill : Skill
    {
        protected AssassinSkill(string name, string description, int mp)
            : base(name, description, mp) { }
    }

    public abstract class MonsterSkill : Skill
    {
        protected MonsterSkill(string name, string description, int mp)
            : base(name, description, mp) { }
    }

}
