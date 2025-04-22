using System.Dynamic;

namespace SpartaDungeon
{
    internal interface IBattleUnit
    {
        string Name { get; }    //이름
        int Level { get; }      //레벨
        int FullHP { get; }     //최대 체력
        int CurrentHP { get; }     //현재 체력
        int Attack { get; }     //공격력
        int Defense { get; }    //방어력
        bool IsDead { get; }    //사망 여부
        float critChance { get; }  //치명타 확률
        float evadeChance { get; }     //회피 확률

        void OnDamage(int damage);  //피격
        void RecoverHP(int hp);     //회복 
        void OnDie();       //사망
    }
}