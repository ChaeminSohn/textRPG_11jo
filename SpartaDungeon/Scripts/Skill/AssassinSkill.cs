using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class LuckySeven : AssassinSkill
    {
        public float Damage { get; private set; }

        public LuckySeven() : base("럭키 세븐", "MP 14 소비, 적 하나에게 100%(+ 치명타율) 데미지 두번 공격  ", 14)
        {
            Damage = 1.0f;
        }

        public override void Activate(BattleUnit attacker, BattleUnit defender, Monster[] monsters)
        {
            Console.Write($"\n\nLv.{attacker.Level} [{attacker.Name}] 의 ");
            ColorFont.Write($"[{Name}]\n\n", Color.Blue);

            attacker.DamageResult(defender, Damage + attacker.CritChance);
            attacker.DamageResult(defender, Damage + attacker.CritChance);

            attacker.UseMP(MP);
        }
    }

    public class SavageBlow : AssassinSkill
    {
        public float Damage { get; private set; }

        public SavageBlow() : base("새비지 블로우", "MP 30 소비, 적 하나에게  80%  데미지를 주고 랜덤 적에게 4번 60% 데미지", 30)
        {
            Damage = 0.8f;
        }

        public override void Activate(BattleUnit attacker, BattleUnit defender, Monster[] monsters)
        {
            Console.Write($"\n\nLv.{attacker.Level} [{attacker.Name}] 의 ");
            ColorFont.Write($"[{Name}]\n\n", Color.Blue);

            attacker.DamageResult(defender, Damage); // 적 하나에게  80%  데미지

            RandomTargetAttack(attacker, monsters, 4, 0.6f);

            attacker.UseMP(MP);
        }
    }
}
