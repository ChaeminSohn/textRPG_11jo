using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    //플레이어의 인벤토리를 구현하는 클래스
    //플레이어가 소유한 모든 아이템 관리
    //장비 아이템들의 장착 관리
    public class Inventory
    {
        Player player;
        public List<ITradable> Items { get; private set; } = new List<ITradable>(16); //보유중인 모든 아이템
        public List<ITradable> Equipments { get; private set; } = new List<ITradable>(); //장비 아이템
        public List<ITradable> Usables { get; private set; } = new List<ITradable>(); //소비 아이템
        public List<ITradable> Others { get; private set; } = new List<ITradable>(); //기타 아이템
        public Dictionary<EquipType, Equipment?> EquippedItems =  //플레이어가 장비중인 아이템 
             Enum.GetValues(typeof(EquipType))
                .Cast<EquipType>()
                    .ToDictionary(type => type, type => (Equipment?)null);

        public event Action? OnEquipChanged;    //플레이어 장비 변환 이벤트
        public void SetOwner(Player player)     //인벤토리 주인(플레이어) 할당
        {
            this.player = player;
        }

        //인벤토리에 아이템 추가
        public void AddItem(ITradable item)  //객체를 직접 전달(장비 아이템)
        {
            Items.Add(item);
            if (item is Equipment equipment)
            {
                Equipments.Add(equipment);
                if (equipment.IsEquipped)   //장착중인 장비인 경우
                {   //장착 해제 및 장착 목록에서 제거
                    EquippedItems[equipment.EquipType] = equipment;
                }
            }
            else if (item is Usable usable)
            {
                Usables.Add(usable);
            }
            else if (item is OtherItem other)
            {
                Others.Add(other);
            }
        }

        public void AddItem(ItemInfo itemInfo)  //정보만 전달하여 객체를 생성할지 결정(소비, 기타 아이템)
        {
            switch (itemInfo.ItemType)
            {
                case ItemType.Equipment:
                    Equipment equipItem = new Equipment(itemInfo);
                    Equipments.Add(equipItem);
                    Items.Add(equipItem);
                    break;
                case ItemType.Usable:   //소비, 기티 아이템은 이미 가지고 있는지 확인
                    ITradable? existingUsable = Usables.FirstOrDefault(item => item.ID == itemInfo.ID);
                    if (existingUsable != null)   //이미 가지고 있는 경우
                    {
                        ((Usable)existingUsable).ChangeItemCount(itemInfo.ItemCount);      //개수만 추가
                    }
                    else
                    {
                        Usable usableItem = new Usable(itemInfo);
                        Usables.Add(usableItem);
                        Items.Add(usableItem);
                    }
                    break;
                case ItemType.Other:
                    ITradable? existingOther = Others.FirstOrDefault(item => item.ID == itemInfo.ID);
                    if (existingOther != null)   //이미 가지고 있는 경우
                    {
                        ((OtherItem)existingOther).ChangeItemCount(itemInfo.ItemCount);      //개수만 추가
                    }
                    else
                    {
                        OtherItem otherItem = new OtherItem(itemInfo);
                        Others.Add(otherItem);
                        Items.Add(otherItem);
                    }
                    break;
            }
        }
        public void RemoveItem(ITradable item)  //인벤토리에서 아이템 제거
        {
            Items.Remove(item);
            if (item is Equipment equipment)
            {
                Equipments.Remove(equipment);
                if (equipment.IsEquipped)   //장착중인 장비인 경우
                {   //장착 해제 및 장착 목록에서 제거
                    equipment.UnEquip();
                    EquippedItems[equipment.EquipType] = null;
                    OnEquipChanged?.Invoke();
                }
            }
            else if (item is Usable usable)
            {
                Usables.Remove(usable);
            }
            else if (item is OtherItem other)
            {
                Others.Remove(other);
            }
        }

        //장비, 소비, 기타 아이템을 따로 표기하는 기능은 아직 구현 안됨
        public void ShowInventory()     // 2: 인벤토리 창 - 모든 아이템 표시
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("<인벤토리>");
                Console.WriteLine("보유 중인 아이템을 확인할 수 있습니다.");

                Console.WriteLine("\n1. 장비 아이템");
                Console.WriteLine("2. 소비 아이템");
                Console.WriteLine("3. 기타 아이템");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 1:
                        ControlEquipments();
                        break;
                    case 2:
                        ControlUsables();
                        break;
                    case 3:
                        ControlOthers();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Utils.Pause(false);
                        break;
                }
            }
        }

        public void ControlEquipments()     //장비 아이템 장착 관리
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장비 아이템");
                Console.WriteLine("장비 아이템을 장착/해제할 수 있습니다.");

                Console.WriteLine("\n[아이템 목록]");
                for (int i = 0; i < Equipments.Count; i++)
                {
                    if (((Equipment)Equipments[i]).IsEquipped)
                    {
                        Console.Write($"- {i + 1} [E]");
                    }
                    else
                    {
                        Console.Write($"- {i + 1}    ");
                    }
                    Equipments[i].ShowInfo(false);
                }

                Console.WriteLine("\n0. 나가기");
                Console.Write("\n장착/해제할 아이템 번호를 입력하세요. ");

                int playerInput = Utils.GetPlayerInput();

                if (playerInput == 0) //인풋 0 : 나가기
                {
                    return;
                }
                else if (playerInput > Equipments.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("\n잘못된 입력입니다.");
                    Utils.Pause(false);
                }
                else
                {
                    Equipment selected = (Equipment)Equipments[playerInput - 1];
                    int equipIndex = (int)selected.EquipType;   //무기 : 0, 방어구 : 1

                    if (selected.IsEquipped)    //이미 장착된 경우
                    {
                        selected.UnEquip();     //장착 해제
                        EquippedItems[selected.EquipType] = null;
                    }
                    else
                    {
                        //같은 종류 장비가 이미 장착되어 있으면 해제
                        if (EquippedItems[selected.EquipType] != null)
                        {
                            EquippedItems[selected.EquipType]!.UnEquip();
                        }

                        selected.Equip();
                        EquippedItems[selected.EquipType] = selected;
                    }
                    OnEquipChanged?.Invoke();
                }
            }
        }

        public void ControlUsables()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 소비 아이템");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("\n[아이템 목록]");
                for (int i = 0; i < Usables.Count; i++)
                {
                    Console.Write($"- {i + 1}.  ");
                    Usables[i].ShowInfo(false);
                }
                Console.WriteLine("\n0. 나가기");
                Console.Write("\n사용할 아이템을 선택하세요.");

                int playerInput = Utils.GetPlayerInput();

                if (playerInput == 0) //인풋 0 : 나가기
                {
                    return;
                }
                else if (playerInput > Usables.Count || playerInput == -1)
                {   //인풋이 아이템 개수보다 크거나 완전 잘못된 값일 때
                    Console.WriteLine("\n잘못된 입력입니다.");
                    Utils.Pause(false);
                }
                else
                {
                    Usable selected = (Usable)Usables[playerInput - 1];
                    Console.Clear();
                    Console.WriteLine($"{selected.Name} 을(를) 사용하시겠습니까?");
                    Console.WriteLine("\n1. 사용");
                    Console.WriteLine("0. 취소");

                    switch (Utils.GetPlayerInput())
                    {
                        case 0:
                            continue;
                        case 1:
                            selected.Use(player); //아이템 사용
                            Utils.Pause(true);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Utils.Pause(false);
                            break;
                    }
                }
            }
        }

        public void ControlOthers()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 기타 아이템");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("\n[아이템 목록]");
            foreach (ITradable item in Others)
            {
                Console.Write("- ");
                item.ShowInfo(false);
            }

            Console.WriteLine("\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.");
            int playerInput = Utils.GetPlayerInput();

            if (playerInput == 0) //인풋 0 : 나가기
            {
                return;
            }
        }
    }
}
