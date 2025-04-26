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
        List<ITradable> equipments = new List<ITradable>();     //판매하는 모든 장비 아이템
        List<ITradable> weaponList = new List<ITradable>();
        List<ITradable> armorList = new List<ITradable>();
        List<ITradable> headList = new List<ITradable>();
        List<ITradable> shoeList = new List<ITradable>();
        List<ITradable> gloveList = new List<ITradable>();
        List<ITradable> subWeaponList = new List<ITradable>();

        List<ITradable> usables = new List<ITradable>();   //판매하는 소비 아이템 목록
        List<ITradable> others = new List<ITradable>();     //판매하는 기타 아이템 목록
        Player player;

        public Shop(Player player)      //새 게임 생성자
        {
            this.player = player;
            foreach (ItemInfo itemInfo in ItemDataBase.ShopItems)
            {
                ITradable createdItem = ItemFactory.Create(itemInfo);
                switch (itemInfo.ItemType)
                {
                    case ItemType.Equipment:
                        equipments.Add(createdItem);
                        break;
                    case ItemType.Usable:
                        usables.Add(createdItem);
                        break;
                    case ItemType.Other:
                        others.Add(createdItem);
                        break;
                }
            }
            Items.AddRange(equipments);
            Items.AddRange(usables);
            Items.AddRange(others);
            EquipmentDivision(equipments);
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
            EquipmentDivision(equipments);
        }

        public void EquipmentDivision(List<ITradable> items) // 장비 아이템 분배
        {
            foreach (ITradable item in items)
            {
                switch (item.EquipType)
                {
                    case EquipType.Weapon:
                        weaponList.Add(item);
                        break;
                    case EquipType.Armor:
                        armorList.Add(item);
                        break;
                    case EquipType.Head:
                        headList.Add(item);
                        break;
                    case EquipType.Glove:
                        gloveList.Add(item);
                        break;
                    case EquipType.Shoe:
                        shoeList.Add(item);
                        break;
                    case EquipType.SubWeapon:
                        subWeaponList.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }


        public void ShowShop() // 3: 상점 창
        {
            SoundManager.PlayBgm("bgm_shop.mp3");
            while (true)
            {
                //상점 인터페이스 표시
                Console.Clear();
                Console.WriteLine("<자유시장>");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

                //플레이어 입력 받기
                Console.WriteLine("\n1. 무기 상점");
                Console.WriteLine("2. 방어구 상점");
                Console.WriteLine("3. 투구 상점");
                Console.WriteLine("4. 신발 상점");
                Console.WriteLine("5. 장갑 상점");
                Console.WriteLine("6. 보조무기 상점");
                Console.WriteLine("7. 소비 아이템");
                Console.WriteLine("8. 기타 아이템");
                ColorFont.Write("\n0. 나가기\n", Color.Magenta);
                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        ShowCategory("무기 상점", weaponList, player.Inventory.Equipments);
                        break;
                    case 2:
                        ShowCategory("방어구 상점", armorList, player.Inventory.Equipments);
                        break;
                    case 3:
                        ShowCategory("투구 상점", headList, player.Inventory.Equipments);
                        break;
                    case 4:
                        ShowCategory("신발 상점", shoeList, player.Inventory.Equipments);
                        break;
                    case 5:
                        ShowCategory("장갑 상점", gloveList, player.Inventory.Equipments);
                        break;
                    case 6:
                        ShowCategory("보조무기 상점", subWeaponList, player.Inventory.Equipments);
                        break;
                    case 7:
                        ShowCategory("소비 아이템", usables, player.Inventory.Usables);
                        break;
                    case 8:
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
                Console.WriteLine($"{player.Meso} 메소");
                ColorFont.Write("\n[아이템 목록]\n", Color.Green);

                foreach (ITradable item in shopItems)
                {
                    Console.Write("- ");
                    item.ShowInfo(true);
                    Console.WriteLine();
                }

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                ColorFont.Write("\n0. 나가기\n", Color.Magenta);
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
                Console.WriteLine($"{player.Meso} 메소");

                ColorFont.Write("\n[아이템 목록]\n", Color.Green);
                //아이템 목록 보여줌
                int index = 1;
                foreach (ITradable item in sellingItems)
                {
                    Console.Write($"- {index++}. ");
                    item.ShowInfo(true);
                    Console.WriteLine();
                }
                ColorFont.Write("\n0. 나가기\n", Color.Magenta);
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
                            Console.WriteLine($"아이템 가격 : {selectedItem.Price} meso , 보유 메소 : {player.Meso} 메소");
                            Console.WriteLine("1. 구매");
                            Console.WriteLine("2. 다시 생각해본다");
                            Console.Write("\n원하시는 행동을 입력해주세요.");
                            switch (Utils.GetPlayerInput())
                            {
                                case 1:
                                    player.ChangeMeso(-selectedItem.Price);
                                    player.Inventory.AddItem(selectedItem);
                                    selectedItem.OnTrade();
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
                    else if (selectedItem.ItemType == ItemType.Usable ||
                        selectedItem.ItemType == ItemType.Other)   //소비, 기타 아이템 - 한번에 여러 개수 구매 가능
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
            Console.WriteLine($"몇 개를 구매하시겠습니까? (보유: {player.Meso} 메소)");
            Console.Write(">> ");

            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0 || count > itemCount)
            {

                Console.WriteLine("잘못된 수량입니다. 숫자를 정확히 입력해주세요.");
                Utils.Pause(false);
                return;
            }

            int totalPrice = item.Price * count;

            if (player.Meso >= totalPrice)      //돈이 충분한 경우
            {
                if (item is Usable usableItem)
                {
                    ITradable? existingItem = player.Inventory.Usables.FirstOrDefault(item => item.ID == usableItem.ID);
                    if (existingItem != null)   //아이템이 인벤토리에 이미 존재하는 경우
                    {
                        ((Usable)existingItem).ChangeItemCount(count);  //개수만 추가           
                    }
                    else    //인벤토리에 존재하지 않는 경우
                    {
                        player.Inventory.AddItem(usableItem.CloneItem(count));     //새로운 객체 복사하여 전달
                    }
                    usableItem.ChangeItemCount(-count);     //상점 객체는 개수 감소
                    if (usableItem.ItemCount == 0)  //개수가 0이 된 경우
                    {
                        usables.Remove(usableItem);     //판매 목록에서 삭제
                        Items.Remove(usableItem);
                    }
                }
                else if (item is OtherItem otherItem)
                {
                    ITradable? existingItem = player.Inventory.Others.FirstOrDefault(item => item.ID == otherItem.ID);
                    if (existingItem != null)   //아이템이 인벤토리에 이미 존재하는 경우
                    {
                        ((OtherItem)existingItem).ChangeItemCount(count);  //개수만 추가           
                    }
                    else    //인벤토리에 존재하지 않는 경우
                    {
                        player.Inventory.AddItem(otherItem.CloneItem(count));     //새로운 객체 복사하여 전달
                    }
                    otherItem.ChangeItemCount(-count);
                    if (otherItem.ItemCount == 0)
                    {
                        others.Remove(otherItem);
                        Items.Remove(otherItem);
                    }
                }
                player.ChangeMeso(-totalPrice);
                Console.WriteLine($"{item.Name} {count}개를 구매했습니다.");
            }
            else    //돈이 부족한 경우
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
                Console.WriteLine($"{player.Meso} 메소");

                ColorFont.Write("\n[아이템 목록]\n", Color.Green);
                //아이템 목록 보여줌
                int index = 1;
                foreach (ITradable item in sellingItems)
                {
                    Console.Write($"- {index++}. ");
                    item.ShowInfo(false);
                }
                ColorFont.Write("\n0. 나가기\n", Color.Magenta);
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
                else    //올바른 아이템 번호 입력
                {
                    ITradable selectedItem = sellingItems[playerInput - 1];     //선택된 아이템
                    if (selectedItem.ItemType == ItemType.Equipment)
                    {
                        //판매 시 80% 절감
                        int sellPrice = (int)(selectedItem.Price * 0.8);
                        //판매 확정 단계
                        Console.WriteLine($"\n{selectedItem.Name} : {sellPrice} 메소");
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
                    else if (selectedItem.ItemType == ItemType.Usable ||
                        selectedItem.ItemType == ItemType.Other)
                    {
                        SellMultipleItems(selectedItem);
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

            if (item is Usable usableItem)  //소비 아이템
            {
                ITradable? existingItem = usables.FirstOrDefault(item => item.ID == usableItem.ID);
                if (existingItem != null)   //아이템이 이미 상점에 존재하는 경우
                {
                    ((Usable)existingItem).ChangeItemCount(count);      //개수만 추가
                }
                else    //상점에 존재하지 않는 경우
                {
                    usables.Add(usableItem.CloneItem(count));   //새로운 객체 복사하여 추가
                }
                usableItem.ChangeItemCount(-count);
                if (usableItem.ItemCount == 0)  //개수가 0이 된 경우
                {
                    player.Inventory.RemoveItem(usableItem);    //인벤토리에서 삭제
                }
            }
            else if (item is OtherItem otherItem)   //기타 아이템
            {
                ITradable? existingItem = others.FirstOrDefault(item => item.ID == otherItem.ID);
                if (existingItem != null)   //아이템이 이미 상점에 존재하는 경우
                {
                    ((OtherItem)existingItem).ChangeItemCount(count);      //개수만 추가
                }
                else    //상점에 존재하지 않는 경우
                {
                    others.Add(otherItem.CloneItem(count));   //새로운 객체 복사하여 추가
                }
                otherItem.ChangeItemCount(-count);
                if (otherItem.ItemCount == 0)   //개수가 0이 된 경우
                {
                    player.Inventory.RemoveItem(otherItem); //인벤토리에서 삭제
                }
            }

            player.ChangeMeso(totalPrice);
            Console.WriteLine($"{item.Name} {count}개를 판매하여 {totalPrice}메소를 얻었습니다.");
            Utils.Pause(true);
        }
    }
}
