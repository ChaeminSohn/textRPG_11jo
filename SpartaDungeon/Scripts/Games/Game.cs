using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Game
    {
        Inventory inventory;
        Player player;
        Shop shop;
        Dungeon dungeon;
        Inn inn = new Inn();
        bool isGameOver;
        QuestMenu questMenu;

        public void GameStart()     //게임 시작
        {
            LoadDatabases();
            SoundManager.PlayBgm("bgm_title.mp3");
            if (File.Exists(PathConstants.SaveFilePath))  //세이브 파일이 존재할 경우
            {
                Console.Clear();
                Console.WriteLine(DotArt.asciiArt_0);
                Console.WriteLine("세이브 파일이 존재합니다. 이어서 게임을 시작합니다.");
                Utils.Pause(true);
                LoadGame();
            }
            else    //세이브 파일이 존재하지 않을 경우
            {
                Console.Clear();
                Console.WriteLine(DotArt.asciiArt_0);
                Console.WriteLine("세이브 파일이 없습니다. 새로운 게임을 시작합니다.");
                Utils.Pause(true);
                StartNewGame();
            }
            SoundManager.PlayBgm("bgm_town.mp3");
            MainGameLoop();
        }

        private void LoadDatabases()
        {
            ItemDataBase.Load(PathConstants.ItemConfigPath);
            MonsterDataBase.Load(PathConstants.ResourceFolder);
            QuestDataBase.Load(PathConstants.QuestConfigPath);
        }

        private void StartNewGame()
        {
            string playerName = PlayerCreator.GetPlayerName();
            Job job = PlayerCreator.SelectJob();
            inventory = new Inventory();
            inventory.SetOwner(player);
            player = new Player(playerName, job, inventory);
            shop = new Shop(player);
            dungeon = new Dungeon(player);
            inn = new Inn();
            questMenu = new QuestMenu(player);
            isGameOver = false;
        }
        private void LoadGame()
        {
            if (!SaveLoadManager.TryLoadGame(out GameSaveData saveData))
            {
                return;
            }

            inventory = new Inventory();
            foreach (ItemInfo item in saveData.InventoryItemData)
            {   //인벤토리 재구성
                inventory.AddItem(ItemFactory.Create(item));
            }

            List<ITradable> shopItems = new List<ITradable>();
            foreach (ItemInfo item in saveData.ShopItemData)
            {   //상점 재구성
                shopItems.Add(ItemFactory.Create(item));
            }
            player = new Player(saveData.PlayerData, inventory);
            //퀘스트 재구성
            questMenu = new QuestMenu(player, saveData.QuestData);

            player.RestoreAfterLoad();
            player.UpdatePlayerStats();
            shop = new Shop(player, shopItems);
            dungeon = new Dungeon(player);
            inn = new Inn();

            MonsterDataBase.MonsterKillCount = new Dictionary<int, int>(saveData.MonsterKillData);
            inventory.SetOwner(player);
        }

        private void MainGameLoop()
        {
            while (!isGameOver)
            {
                TownAction();

                if (player.IsDead)
                {
                    GameOver();
                }
            }

            Console.Clear();
            Console.WriteLine("게임이 종료되었습니다.");
        }

        public void TownAction()
        {
            Console.Clear();
            Console.WriteLine("메이플 월드에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 자유롭게 활동을 할 수 있습니다.\n\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 자유시장");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 게시판");
            Console.WriteLine("7. 게임 종료\n");
            Console.Write("\n원하시는 행동을 입력해주세요.");

            switch (Utils.GetPlayerInput())
            {
                case 1:     //상태창 표시
                    player.ShowState();
                    Console.Clear();
                    break;
                case 2:     //인벤토리 열기
                    inventory.ShowInventory();
                    Console.Clear();
                    break;
                case 3:     //상점 열기
                    shop.ShowShop();
                    break;
                case 4:     //던전 입장
                    dungeon.Enter();
                    break;
                case 5:     //휴식하기
                    inn.EnterInn(player);
                    break;
                case 6:     //퀘스트 게시판 열기
                    questMenu.OpenQuestBoard();
                    break;
                case 7:     //게임 종료하기
                    ExitGame();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    break;
            }
        }

        void ExitGame()     // 6: 게임 종료 액션
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("게임을 종료하시겠습니까?");
                Console.WriteLine("진행상황은 자동으로 저장됩니다.");
                Console.WriteLine("\n1. 계속하기");
                Console.WriteLine("2. 게임 종료");

                Console.Write("\n원하시는 행동을 입력해주세요.");
                switch (Utils.GetPlayerInput())
                {
                    case 1:
                        return;
                    case 2:
                        Console.WriteLine("\n게임을 종료합니다.");
                        List<ItemInfo> inventoryItemData = new List<ItemInfo>();
                        List<ItemInfo> shopItemData = new List<ItemInfo>();

                        foreach (ITradable item in inventory.Items) //인벤토리 아이템 정보 저장
                        {
                            inventoryItemData.Add(item.GetItemInfo());
                        }
                        foreach (ITradable item in shop.Items)      //상점 아이템 정보 저장
                        {
                            shopItemData.Add(item.GetItemInfo());
                        }
                        SaveLoadManager.SaveGame(new GameSaveData(player.GetPlayerData(), inventoryItemData, shopItemData,
                            questMenu.GetQuestSaveData(), new Dictionary<int, int>(MonsterDataBase.MonsterKillCount)));
                        isGameOver = true;
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }
        void GameOver()     //플레이어 사망 시 호출
        {
            File.Delete(PathConstants.SaveFilePath);  //저장 데이터 삭제
            while (true)
            {
                Console.Clear();
                Console.WriteLine("You Died");
                Console.WriteLine("\n1. 다시 시작하기");
                Console.WriteLine("\n2. 게임 종료");
                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 1:
                        StartNewGame();
                        return;
                    case 2:
                        Console.WriteLine("\n게임을 종료합니다.");
                        isGameOver = true;
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

    }
}
