using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class MagicClaw : MageSkill
    {
        public float Damage { get; private set; }

        public MagicClaw() : base("매직클로", "MP 20 소비, 적 하나에 데미지 150% 두번 공격", 20)
        {
            Damage = 1.5f;
        }

        public override void Activate(IBattleUnit attacker, IBattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\nLv.{attacker.Level} [{attacker.Name}] 의 [{Name}]!\n");

            attacker.DamageResult(defender, Damage);
            attacker.DamageResult(defender, Damage);
        }
    }

    public class Thunderbolt : MageSkill
    {
        public float Damage { get; private set; }

        public Thunderbolt() : base("썬더 볼트", "MP 40 소비, 모든 적에게 200% 데미지", 40)
        {
            Damage = 2.0f;
        }

        public override void Activate(IBattleUnit attacker, IBattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\nLv.{attacker.Level} [{attacker.Name}] 의 [{Name}]!\n");

            foreach (Monster monster in monsters)
            {
                if (monster.IsDead == false)
                    attacker.DamageResult(monster, Damage);
            }
        }
    }
}
