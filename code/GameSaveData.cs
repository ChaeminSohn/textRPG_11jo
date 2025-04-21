namespace SpartaDungeon
{
    //게임 내 유동적인 정보들은 전부 플레이어와 아이템에 존재하기에, 이 둘에 대한 데이터만 불러오면 됨
    //객체를 직접 저장하기엔 접근 제한자 때문에 곤란한 부분이 많아서 중요 데이터만 빼와서 저장하는 식으로 구현
    public class GameSaveData   //게임 데이터 저장용 래퍼 클래스
    {
        public PlayerData PlayerData { get; set; }  //플레이어의 정보를 담는 객체
        public List<ItemInfo> ItemData { get; set; }    //모든 아이템들의 정보를 담는 객체

        public GameSaveData(PlayerData playerData, List<ItemInfo> itemData)
        {
            PlayerData = playerData;
            ItemData = itemData;
        }
    }
}