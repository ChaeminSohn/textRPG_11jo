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
        string folderPath = @"..\..\..\resources";
        bool isGameOver;
        string savePath;//저장 파일 경로
        List<Monster> monsterList = new List<Monster>();

        public void GameStart()     //게임 시작
        {
            savePath = Path.Combine(folderPath, "saveData.json");
            ItemDataBase.Load(@"..\..\..\resources/items_config.json");

            if (File.Exists(savePath))  //세이브 파일이 존재할 경우
            {
                Console.Clear();

                Console.WriteLine("세이브 파일이 존재합니다. 이어서 게임을 시작합니다.");
                LoadData();
                Utils.Pause(true);
            }
            else    //세이브 파일이 존재하지 않을 경우
            {

                Console.Clear();
                Console.WriteLine("세이브 파일이 없습니다. 새로운 게임을 시작합니다.");
                Utils.Pause(true);
                NewGameSetting();

            }

            while (!isGameOver)     //게임이 종료될 때 까지 반복
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
        void NewGameSetting()   //새로운 게임 설정
        {
            //몬스터 데이터 불러오기
            if (ConfigLoader.TryLoad<MonsterConfig>(@"..\..\..\resources/monster_config.json", out var monsterConfig))
            {
                monsterList = monsterConfig.Monster;
            }
            else
            {
                Console.WriteLine("몬스터 데이터를 불러오지 못했습니다.");
                Utils.Pause(false);
            }

            string playerName = GetNameFromPlayer(); ;
            Job playerJob = GetJobFromPlayer(); ;

            inventory = new Inventory();

            player = new Player(playerName, playerJob, inventory);
            //player.OnPlayerDie += GameOver;
            shop = new Shop(player);
            dungeon = new Dungeon(player, monsterList);

            isGameOver = false;
        }

        void SaveData()     //게임 저장
        {
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
            Utils.Pause(false);
            GameSaveData gameSaveData = new GameSaveData(player.GetPlayerData(), inventoryItemData, shopItemData);
            string json = JsonSerializer.Serialize(gameSaveData);
            File.WriteAllText(savePath, json);
        }

        void LoadData()     //게임 불러오기
        {

            if (!ConfigLoader.TryLoad<GameSaveData>(savePath, out var config))
            {
                Console.WriteLine("저장 데이터 불러오기 실패");
                Utils.Pause(false);
                return;
            }
            //몬스터 데이터 불러오기
            if (ConfigLoader.TryLoad<MonsterConfig>(@"..\..\..\resources/monster_config.json", out var monsterConfig))
            {
                monsterList = monsterConfig.Monster;
            }
            else
            {
                Console.WriteLine("몬스터 데이터를 불러오지 못했습니다.");
                Utils.Pause(false);
            }
            inventory = new Inventory();
            foreach (ItemInfo info in config.InventoryItemData)    //인벤토리 아이템 불러오기
            {
                inventory.AddItem(ItemFactory.CreateItem(info));
            }
            List<ITradable> itemList = new List<ITradable>();
            foreach (ItemInfo info in config.ShopItemData)      //상점 아이템 불러오기
            {
                itemList.Add(ItemFactory.CreateItem(info));
            }

            player = new Player(config.PlayerData, inventory);
            player.RestoreAfterLoad();
            player.UpdatePlayerStats();
            shop = new Shop(player, itemList);
            dungeon = new Dungeon(player, monsterList);
            isGameOver = false;
        }

        QuestMenu questMenu = new QuestMenu();

        public void TownAction()
        {
            Console.Clear();
            Console.WriteLine("메이플 월드에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n\n");

            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 게시판");
            Console.WriteLine("7. 게임 종료\n");
            Console.Write("\n원하시는 행동을 입력해주세요.");

            switch (Utils.GetPlayerInput())
            {
                case 1:     //상태창 표시
                    ShowState();
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
                        SaveData();
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
            File.Delete(savePath);  //저장 데이터 삭제
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
                        NewGameSetting();
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



        public void ShowState() // 상태 창
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("<상태 보기>");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");

                //플레이어 입력 받기
                Console.WriteLine("\n1. 캐릭터 정보 ");
                Console.WriteLine("2. 처치 현황");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.");
                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        player.ShowPlayerInfo();
                        Utils.Pause(true);
                        break;
                    case 2:
                        ShowKillCount();
                        Utils.Pause(true);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;

                }
            }
        }


        public void ShowKillCount() // 킬 카운트 확인
        {
            Console.Clear();
            Console.WriteLine("<상태 보기>");
            Console.WriteLine("처치 현황이 표시됩니다.");
            Console.WriteLine("\n[처치 현황]");
            foreach (var kv in player.monsterKillCounts)
            {
                var info = monsterList.FirstOrDefault(m => m.Id == kv.Key); // 키 확인
                var name = info != null ? info.Name : $"ID:{kv.Key}"; // 이름 확인
                Console.WriteLine($"{name} : {kv.Value}마리");
            }
        }

        string GetNameFromPlayer()  //이름 입력
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("메이플 월드에 오신 걸 환영합니다!");
                Console.WriteLine("플레이어 이름을 입력해주세요.\n");
                string input = Console.ReadLine();

                // 입력이 null이거나 공백인 경우
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("이름은 비워둘 수 없습니다. 다시 입력해주세요.");
                    Utils.Pause(false);
                    continue;
                }

                // 너무 짧거나 긴 이름인 경우
                if (input.Length < 2 || input.Length > 12)
                {
                    Console.WriteLine("이름은 2자 이상 12자 이하여야 합니다.");
                    Utils.Pause(false);
                    continue;
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"입력하신 이름은 {input} 입니다.");
                    Console.WriteLine("\n1. 확인");
                    Console.WriteLine("2. 취소");


                    switch (Utils.GetPlayerInput())
                    {
                        case 1:
                            return input;
                        case 2:
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Utils.Pause(false);
                            continue;
                    }
                    break;
                }
            }
        }

        Job GetJobFromPlayer()    //직업 입력
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("직업을 선택하세요.\n");
                foreach (Job job in Enum.GetValues(typeof(Job)))
                {
                    Console.WriteLine($"{(int)job + 1}. {Utils.JobDisplayNames[job]}");
                }

                Console.WriteLine();
                int input = Utils.GetPlayerInput();

                if (input >= 1 && input <= 3)   //입력이 1, 2, 3
                {
                    Job selectedJob = (Job)(input - 1);
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"{Utils.JobDisplayNames[selectedJob]}을 선택하시겠습니까?");
                        Console.WriteLine("\n1. 확인");
                        Console.WriteLine("2. 취소");

                        switch (Utils.GetPlayerInput())
                        {
                            case 1:
                                return selectedJob;
                            case 2:
                                break;
                            default:
                                Console.WriteLine("잘못된 입력입니다.");
                                Utils.Pause(false);
                                continue;
                        }
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                }

            }
        }
    }
}
