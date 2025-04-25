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

        public void AddItem(ITradable item)
        {
            Items.Add(item);
            if (item is Equipment equipment)
            {
                Equipments.Add(equipment);
                if (equipment.IsEquipped)
                {
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

        public void AddItem(ItemInfo itemInfo)
        {
            switch (itemInfo.ItemType)
            {
                case ItemType.Equipment:
                    Equipment equipItem = new Equipment(itemInfo);
                    Equipments.Add(equipItem);
                    Items.Add(equipItem);
                    break;
                case ItemType.Usable:
                    ITradable? existingUsable = Usables.FirstOrDefault(item => item.ID == itemInfo.ID);
                    if (existingUsable != null)
                    {
                        ((Usable)existingUsable).ChangeItemCount(itemInfo.ItemCount);
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
                    if (existingOther != null)
                    {
                        ((OtherItem)existingOther).ChangeItemCount(itemInfo.ItemCount);
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

        public void RemoveItem(ITradable item)
        {
            Items.Remove(item);
            if (item is Equipment equipment)
            {
                Equipments.Remove(equipment);
                if (equipment.IsEquipped)
                {
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

        public void ControlEquipments()
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

                if (playerInput == 0)
                {
                    return;
                }
                else if (playerInput > Equipments.Count || playerInput == -1)
                {
                    Console.WriteLine("\n잘못된 입력입니다.");
                    Utils.Pause(false);
                }
                else
                {
                    Equipment selected = (Equipment)Equipments[playerInput - 1];
                    int equipIndex = (int)selected.EquipType;

                    if (selected.IsEquipped)
                    {
                        selected.UnEquip();
                        EquippedItems[selected.EquipType] = null;
                    }
                    else
                    {
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

        // ✅ 퀘스트 조건 체크를 위한 아이템 개수 조회
        public int GetItemCount(int itemId)
        {
            var target = Items.FirstOrDefault(i => i.ID == itemId);
            if (target is Usable usable)
                return usable.ItemCount;
            else if (target is OtherItem other)
                return other.ItemCount;
            return target != null ? 1 : 0; // 장비나 존재하는 기타 아이템
        }

        // ✅ 퀘스트 제출을 위한 아이템 차감
        public bool ReduceItemCount(int itemId, int count)
        {
            var target = Items.FirstOrDefault(i => i.ID == itemId);
            if (target is Usable usable && usable.ItemCount >= count)
            {
                usable.ChangeItemCount(-count);
                if (usable.ItemCount == 0) RemoveItem(usable);
                return true;
            }
            else if (target is OtherItem other && other.ItemCount >= count)
            {
                other.ChangeItemCount(-count);
                if (other.ItemCount == 0) RemoveItem(other);
                return true;
            }
            return false;

        }
    }
}

