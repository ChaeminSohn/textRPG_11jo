using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class NormalMonster
    {
        List<Monster> monsters = new List<Monster>() // 이름 레벨 최대체력 현제체력 공격력 방어력
        {
            new Monster("전사미니언", 1, 30, 30, 2, 4),
            new Monster("마법사미니언", 1, 20, 20, 4, 2),
            new Monster("대포미니언", 1, 40, 40, 4, 5),
        };
    }
}
