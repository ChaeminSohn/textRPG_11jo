namespace SpartaDungeon
{
    internal interface IBattleUnit
    {
        string Name { get; }
        int Level { get; }
        int FullHP { get; }
        int CurrentHP { get; }
        int Attack { get; }
        int Defense { get; }

        void OnDamage(int damage);
        void RecoverHP(int hp);
        void OnDie();
    }
}