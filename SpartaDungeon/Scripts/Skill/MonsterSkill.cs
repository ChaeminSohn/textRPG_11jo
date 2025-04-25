using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class Thud : MonsterSkill // 쿵!
    {
        public float Damage { get; private set; }

        public Thud() : base("쿵!", "눈을 번뜩이면 점프한다.", 0)
        {
            Damage = 1.3f;
        }

        public override void Activate(BattleUnit attacker, BattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\n[{attacker.Name}]이 {Description}");
            Console.Write($"Lv.{attacker.Level} [{attacker.Name}] 의 ");
            ColorFont.Write($"[{Name}]\n\n", Color.Red);

            attacker.DamageResult(defender, Damage);
        }
    }

    public class Bboong : MonsterSkill // 뿡야!
    {
        public float Damage { get; private set; }

        public Bboong() : base("뿡야!", "눈을 번뜩이면 돌진한다.", 0)
        {
            Damage = 1.3f;
        }

        public override void Activate(BattleUnit attacker, BattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\n[{attacker.Name}]이 {Description}");
            Console.Write($"Lv.{attacker.Level} [{attacker.Name}] 의 ");
            ColorFont.Write($"[{Name}]\n\n", Color.Red);

            attacker.DamageResult(defender, Damage);
        }
    }

    public class Fireball : MonsterSkill // 파이어볼
    {
        public float Damage { get; private set; }

        public Fireball() : base("파이어볼", "불덩이를 던진다.", 0)
        {
            Damage = 0.9f;
        }

        public override void Activate(BattleUnit attacker, BattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\n[{attacker.Name}]이 {Description}");
            Console.Write($"Lv.{attacker.Level} [{attacker.Name}] 의 ");
            ColorFont.Write($"[{Name}]\n\n", Color.Red);

            attacker.DamageResult(defender, Damage);
            attacker.DamageResult(defender, Damage);
        }
    }

    public class BlazeOfRage : MonsterSkill // 격노의 불길
    {
        public float Damage { get; private set; }

        public BlazeOfRage() : base("격노의 불길", "격노한다.", 0)
        {
            Damage = 0.7f;
        }

        public override void Activate(BattleUnit attacker, BattleUnit defender, Monster[] monsters)
        {
            Console.WriteLine($"\n\n[{attacker.Name}]이 {Description}");
            Console.Write($"Lv.{attacker.Level} [{attacker.Name}] 의 ");
            ColorFont.Write($"[{Name}]\n\n", Color.Red);

            attacker.DamageResult(defender, Damage);
            attacker.DamageResult(defender, Damage);
            attacker.DamageResult(defender, Damage);
        }
    }
}
