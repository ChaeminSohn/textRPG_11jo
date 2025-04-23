using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class PawerStrike : WarriorSkill
    {
        public PawerStrike() : base("파워 스트라이크", "MP 12 소비, 적 하나에 데미지 260%", 12, 2.6f) { }
    }

    public class SlashBlust : WarriorSkill
    {
        public SlashBlust() : base("슬래시 블러스트", "MP 30 소비, 모든 적에게 데미지 130%", 30, 1.3f) { }
    }

    public class MagicClaw : MageSkill
    {
        public MagicClaw() : base("매직클로", "MP 20 소비, 적 하나에 데미지 150% 두번 공격", 20, 1.5f) { }
    }

    public class Thunderbolt : MageSkill
    {
        public Thunderbolt() : base("썬더 볼트", "MP 40 소비, 모든 적에게 200% 데미지", 40, 2.0f) { }
    }

    public class ArrowBlow : ArcherSkill
    {
        public ArrowBlow() : base("애로우 블로우", "MP 14 소비, 적 하나에 데미지 260%", 14, 2.6f) { }
    }

    public class ArrowBomb : ArcherSkill
    {
        public ArrowBomb() : base("애로우 봄", "MP 28 소비, 모든 적에게 150% 데미지", 28, 1.5f) { }
    }
}
