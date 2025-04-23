using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public class PawerStrike : WarriorSkill
    {
        private int damage;

        public PawerStrike() : base("파워 스트라이크", "적을 관통하는 검기를 정면으로 날려 대미지를 준다.", 20)
        {
            damage = 10;
        }

        protected override void Activate(Player player, Monster target)
        {
            int damage = this.damage + player.Attack;
        }
    }
}
