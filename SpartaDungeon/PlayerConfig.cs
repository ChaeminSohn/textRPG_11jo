namespace SpartaDungeon
{
    //각 직업별 PlayerData의 초기 설정값을 저장하는 클래스
    //직업에 따라 기본 체력, 공격력, 방어력 값이 다르게 설정된다.
    //골드, 경험치 상한선, 최대 레벨도 다르게 설정 가능함
    internal class PlayerConfig
    {
        public PlayerData? BaseWarriorData { get; set; }
        public PlayerData? BaseMageData { get; set; }
        public PlayerData? BaseArcherData { get; set; }

    }
}