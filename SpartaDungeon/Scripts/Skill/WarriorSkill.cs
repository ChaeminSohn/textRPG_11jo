using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpartaDungeon
{
    public class PawerStrike : WarriorSkill
    {
        public float Damage { get; private set; }

        public PawerStrike() : base("파워 스트라이크", "MP 12 소비, 적 하나에 데미지 260%", 12)
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

    public class SlashBlust : WarriorSkill
    {
        public float Damage { get; private set; }

        public SlashBlust() : base("슬래시 블러스트", "MP 30 소비, 모든 적에게 데미지 130%", 30)
        {
            Damage = 1.3f;
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
