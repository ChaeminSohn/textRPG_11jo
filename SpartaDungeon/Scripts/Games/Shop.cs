using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpartaDungeon
{
    internal class Shop
    {
        public List<ITradable> Items { get; private set; } = new List<ITradable>();      //판매하는 모든 아이템
        List<ITradable> equipments = new List<ITradable>();     //판매하는 장비 아이템
        List<ITradable> usables = new List<ITradable>();   //판매하는 소비 아이템 목록
        List<ITradable> others = new List<ITradable>();     //판매하는 기타 아이템 목록
        Player player;

        public Shop(Player player)      //새 게임 생성자
        {
            this.player = player;
            foreach (ItemInfo itemInfo in ItemDataBase.ShopItems)
            {
                switch (itemInfo.ItemType)     //아이템 분류 작업
                {
                    case ItemType.Equipment:
                        equipments.Add(new Equipment(itemInfo));
                        break;
                    case ItemType.Usable:
                        usables.Add(new Usable(itemInfo));
                        break;
                    case ItemType.Other:
                        others.Add(new OtherItem(itemInfo));
                        break;
                }
            }
            Items.AddRange(equipments);
            Items.AddRange(usables);
            Items.AddRange(others);
        }
        public Shop(Player player, List<ITradable> items)   //게임 불러오기 생성자
        {
            this.player = player;
            Items = items;
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
                        ShowCategory("장비 아이템", equipments, player.Inventory.Equipments);
                        break;
                    case 2:
                        ShowCategory("소비 아이템", usables, player.Inventory.Usables);
                        break;
                    case 3:
                        ShowCategory("기타 아이템", others, player.Inventory.Others);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        private void ShowCategory(string categoryName, List<ITradable> shopItems, List<ITradable> playerItems)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"<{categoryName}>");
                Console.WriteLine("\n[보유 메소]");
                Console.WriteLine($"{player.Meso} G");
                Console.WriteLine("\n[아이템 목록]\n");

                foreach (ITradable item in shopItems)
                {
                    Console.Write("- ");
                    item.ShowInfo(true);
                }

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.");
                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        BuyItems(shopItems);
                        break;
                    case 2:
                        SellItems(playerItems);
                        break;
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
                Console.WriteLine("\n[보유 메소]");
                Console.WriteLine($"{player.Meso} G");

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
                    if (selectedItem.IsSoldOut)  //이미 판매된 아이템인 경우
                    {
                        Console.WriteLine("\n이미 판매된 아이템입니다.");
                        Utils.Pause(false);
                    }
                    else if (selectedItem.ItemType == ItemType.Equipment)   //장비 아이템 - 한번에 하나만 구매 가능
                    {
                        //돈이 충분한 경우
                        if (player.Meso >= selectedItem.Price)
                        {
                            //구매 확정 단계
                            Console.WriteLine($"\n{selectedItem.Name}");
                            Console.WriteLine($"아이템 가격 : {selectedItem.Price} G , 보유 메소 : {player.Meso} G");
                            Console.WriteLine("1. 구매");
                            Console.WriteLine("2. 다시 생각해본다");
                            Console.Write("\n원하시는 행동을 입력해주세요.");
                            switch (Utils.GetPlayerInput())
                            {
                                case 1:
                                    player.ChangeMeso(-selectedItem.Price);
                                    player.Inventory.AddItem(selectedItem);
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
                            Console.WriteLine("\n메소가 부족합니다.");
                            Utils.Pause(false);
                        }
                    }
                    else if (selectedItem.ItemType == ItemType.Usable)   //소비, 기타 아이템 - 한번에 여러 개수 구매 가능
                    {
                        BuyMultipleItems(selectedItem);
                    }
                }
            }
        }

        public void BuyMultipleItems(ITradable item)    //아이템 복수 구매
        {
            int itemCount;
            if (item is Usable usable)
            {
                itemCount = usable.ItemCount;
            }
            else if (item is OtherItem other)
            {
                itemCount = other.ItemCount;
            }
            else
            {
                Console.WriteLine("구매 불가능한 아이템입니다.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"남은 수량 : {itemCount} | 가격 : {item.Price} 메소 ");
            Console.WriteLine($"몇 개를 구매하시겠습니까? (보유: {player.Meso}G)");
            Console.Write(">> ");

            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0 || count > itemCount)
            {

                Console.WriteLine("잘못된 수량입니다. 숫자를 정확히 입력해주세요.");
                Utils.Pause(false);
                return;
            }

            int totalPrice = item.Price * count;

            if (player.Meso >= totalPrice)
            {
                player.ChangeMeso(-totalPrice);
                if (item is Usable usableItem)
                {
                    player.Inventory.AddItem(usableItem.CloneItem(count));
                    usableItem.ChangeItemCount(-count);
                }
                else if (item is OtherItem otherItem)
                {
                    player.Inventory.AddItem(otherItem.CloneItem(count));
                    otherItem.ChangeItemCount(-count);
                }
                Console.WriteLine($"{item.Name} {count}개를 구매했습니다.");
            }
            else
            {
                Console.WriteLine("메소가 부족합니다.");
            }
            Utils.Pause(true);

        }
        public void SellItems(List<ITradable> sellingItems) //아이템 판매 UI
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("<아이템 판매>");
                Console.WriteLine("선택한 아이템을 팔 수 있습니다.");
                Console.WriteLine("\n[보유 메소]");
                Console.WriteLine($"{player.Meso} G");

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
                            player.ChangeMeso(selectedItem.Price);
                            player.Inventory.RemoveItem(selectedItem);
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

        public void SellMultipleItems(ITradable item)
        {
            int itemCount;

            if (item is Usable usable)
            {
                itemCount = usable.ItemCount;
            }
            else if (item is OtherItem other)
            {
                itemCount = other.ItemCount;
            }
            else
            {
                Console.WriteLine("판매할 수 없는 아이템입니다.");
                Utils.Pause(true);
                return;
            }

            Console.Clear();
            Console.WriteLine($"{item.Name} | 보유 수량 : {itemCount} | 개당 판매가 : {(int)(item.Price * 0.8)} 메소");
            Console.WriteLine("몇 개를 판매하시겠습니까?");
            Console.Write(">> ");

            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0 || count > itemCount)
            {
                Console.WriteLine("잘못된 수량입니다. 숫자를 정확히 입력해주세요.");
                Utils.Pause(false);
                return;
            }

            int totalPrice = ((int)(item.Price * 0.8)) * count;

            if (item is Usable usableItem)
            {
                usableItem.ChangeItemCount(-count);
            }
            else if (item is OtherItem otherItem)
            {
                otherItem.ChangeItemCount(-count);
            }

            player.ChangeMeso(totalPrice);
            Console.WriteLine($"{item.Name} {count}개를 판매하여 {totalPrice}G를 얻었습니다.");
            Utils.Pause(true);
        }
    }
}
