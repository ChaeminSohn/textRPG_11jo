using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpartaDungeon
{
    internal class Shop
    {
        List<ITradable> equipments = new List<ITradable>();     //판매하는 장비 아이템
        List<ITradable> usables = new List<ITradable>();   //판매하는 소비 아이템 목록
        List<ITradable> others = new List<ITradable>();     //판매하는 기타 아이템 목록
        ItemType[] itemTypes = (ItemType[])Enum.GetValues(typeof(ItemType));    //모든 아이템 타입을 담는 배열
        Player player;
        public Shop(Player player, List<ITradable> items)
        {
            this.player = player;

            foreach (ITradable item in items)
            {
                switch (item.ItemType)     //아이템 분류 작업
                {
                    case ItemType.Equipment:
                        equipments.Add((Equipment)item);
                        break;
                    case ItemType.Usable:
                        usables.Add((Usable)item);
                        break;
                    case ItemType.Other:
                        others.Add((OtherItem)item);
                        break;
                }
            }
        }

        public void ShowShop() // 3: 상점 창
        {
            while (true)
            {
                //상점 인터페이스 표시
                Console.Clear();
                Console.WriteLine("<상점>");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

                //플레이어 입력 받기
                Console.WriteLine("\n1. 장비 아이템");
                Console.WriteLine("2. 소비 아이템");
                Console.WriteLine("3. 기타 아이템");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.");
                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        ShowEquipments();
                        break;
                    case 2:
                        ShowUsables();
                        break;
                    case 3:
                        ShowOthers();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;

                }
            }
        }

        public void ShowEquipments()    //장비 아이템 판매 
        {
            while (true)
            {
                //상점 인터페이스 표시
                Console.Clear();
                Console.WriteLine("<장비 아이템>");
                Console.WriteLine("인생은 템빨이라는 말이 있죠.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine("\n[아이템 목록]\n");
                //아이템 목록 보여줌
                foreach (ITradable item in equipments)
                {
                    Console.Write("- ");
                    item.ShowInfo(true);
                }
                //플레이어 입력 받기
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.");
                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        BuyItems(equipments);
                        break;
                    case 2:
                        SellItems(player.Inventory.Equipments);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        public void ShowUsables()    //소비 아이템 판매 
        {
            while (true)
            {
                //상점 인터페이스 표시
                Console.Clear();
                Console.WriteLine("죄송합니다. 소비 아이템은 아직 판매하지 않습니다.");
                Console.WriteLine("\n0. 나가기");
                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        public void ShowOthers()    //기타 아이템 판매 
        {
            while (true)
            {
                //상점 인터페이스 표시
                Console.Clear();
                Console.WriteLine("죄송합니다. 기타 아이템은 아직 판매하지 않습니다.");

                Console.WriteLine("\n0. 나가기");
                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        public void BuyItems(List<ITradable> sellingItems) //아이템 구매 UI
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("<아이템 구매>");
                Console.WriteLine("필요한 아이템을 살 수 있습니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{player.Gold} G");

                Console.WriteLine("\n[아이템 목록]");
                //아이템 목록 보여줌
                int index = 1;
                foreach (ITradable item in sellingItems)
                {
                    Console.Write($"- {index++}. ");
                    item.ShowInfo(true);
                }
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n구매할 아이템 번호를 입력하세요. ");
                int playerInput = Utils.GetPlayerInput();

                if (playerInput > sellingItems.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }

                else if (playerInput == 0) //인풋 0 : 나가기
                {
                    return;
                }
                else    //올바른 아이템 번호 입력
                {
                    ITradable selectedItem = sellingItems[playerInput - 1];   //선택된 아이템
                    if (!selectedItem.IsForSale)  //이미 판매된 아이템인 경우
                    {
                        Console.WriteLine("\n이미 판매된 아이템입니다.");
                        Utils.Pause(false);
                    }
                    //돈이 충분한 경우
                    else if (player.Gold >= selectedItem.Price)
                    {
                        //구매 확정 단계
                        Console.WriteLine($"\n{selectedItem.Name}");
                        Console.WriteLine($"아이템 가격 : {selectedItem.Price} G , 보유 골드 : {player.Gold} G");
                        Console.WriteLine("1. 구매");
                        Console.WriteLine("2. 다시 생각해본다");
                        Console.Write("\n원하시는 행동을 입력해주세요.");
                        switch (Utils.GetPlayerInput())
                        {
                            case 1:
                                player.BuyItem(selectedItem);
                                Console.WriteLine("\n구매 감사합니다!");
                                Utils.Pause(true);
                                break;
                            case 2:
                                break;
                            default: //잘못된 입력
                                Console.WriteLine("잘못된 입력입니다.");
                                Utils.Pause(false);
                                break;

                        }
                    }
                    //돈이 부족한 경우
                    else
                    {
                        Console.WriteLine("\n골드가 부족합니다.");
                        Utils.Pause(false);
                    }
                }
            }
        }

        public void SellItems(List<ITradable> sellingItems) //아이템 판매 UI
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("<아이템 판매>");
                Console.WriteLine("선택한 아이템을 팔 수 있습니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{player.Gold} G");

                Console.WriteLine("\n[아이템 목록]");
                //아이템 목록 보여줌
                int index = 1;
                foreach (ITradable item in sellingItems)
                {
                    Console.Write($"- {index++}. ");
                    item.ShowInfo(false);
                }
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n판매할 아이템 번호를 입력하세요. ");
                int playerInput = Utils.GetPlayerInput();

                if (playerInput > sellingItems.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("잘못된 입력입니다.");
                    Utils.Pause(false);
                    continue;
                }
                else if (playerInput == 0) //인풋 0 : 나가기
                {
                    return;
                }
                else
                {
                    ITradable selectedItem = sellingItems[playerInput - 1];
                    //판매 시 80% 절감
                    int sellPrice = (int)(selectedItem.Price * 0.8);
                    //판매 확정 단계
                    Console.WriteLine($"\n{selectedItem.Name} : {sellPrice} G");
                    Console.WriteLine("1. 판매");
                    Console.WriteLine("2. 다시 생각해본다");
                    Console.Write("\n원하시는 행동을 입력해주세요.");
                    switch (Utils.GetPlayerInput())
                    {
                        case 1:
                            player.SellItem(selectedItem, sellPrice);
                            Console.WriteLine("\n판매 감사합니다!");
                            Utils.Pause(true);
                            break;
                        case 2:
                            break;
                        default: //잘못된 입력
                            Console.WriteLine("잘못된 입력입니다.");
                            Utils.Pause(false);
                            break;

                    }
                }
            }
        }

    }
}
