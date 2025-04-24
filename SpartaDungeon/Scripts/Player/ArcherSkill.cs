using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class ArrowBlow : ArcherSkill
    {
        public float Damage { get; private set; }

        public ArrowBlow() : base("애로우 블로우", "MP 14 소비, 적 하나에 데미지 260%", 14)
        {
            Damage = 2.6f;
        }

        public override void Activate(IBattleUnit attacker, IBattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\nLv.{attacker.Level} [{attacker.Name}] 의 [{Name}]!\n");

            attacker.DamageResult(defender, Damage);

            attacker.UseMP(MP);
        }
    }

    public class ArrowBomb : ArcherSkill
    {
        public float Damage { get; private set; }

        public ArrowBomb() : base("애로우 봄", "MP 28 소비, 모든 적에게 150% 데미지", 28)
        {
            Damage = 1.5f;
        }

        public override void Activate(IBattleUnit attacker, IBattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\nLv.{attacker.Level} [{attacker.Name}] 의 [{Name}]!\n");

            foreach (Monster monster in monsters)
            {
                if (monster.IsDead == false)
                    attacker.DamageResult(monster, Damage);
            }

            attacker.UseMP(MP);
        }
    }
}
