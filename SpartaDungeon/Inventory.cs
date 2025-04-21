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
    internal class Inventory
    {
        int inventorySpace = 8;
        public List<ITradable> Items { get; private set; } //보유중인 모든 아이템
        public List<ITradable> Equipments { get; private set; } = new List<ITradable>(); //장비 아이템
        public List<ITradable> Usables { get; private set; } = new List<ITradable>(); //소비 아이템
        public List<ITradable> Others { get; private set; } = new List<ITradable>(); //기타 아이템
        public Equipment[] EquippedItems { get; private set; } //플레이어가 장비중인 아이템
        public event Action? OnEquipChanged;    //플레이어 장비 변환 이벤트
        public Inventory()
        {
            Items = new List<ITradable>(inventorySpace);
            EquippedItems = new Equipment[2];   //0 : 무기 1 :방어구
            foreach (ITradable item in Items)
            {
                /*switch (item.ItemType)
                {
                    case ItemType.Equipment:  //장비 아이템
                        Equipments.Add((Equipment)item);
                        break;
                    case ItemType.Usable:  //소비 아이템   
                        Usables.Add((Usable)item);
                        break;
                    case ItemType.Other:  //기타 아이템  
                        Usables.Add((OtherItem)item);
                        break;
                    default:
                        break;
                }*/
            }
        }

        public void AddItem(ITradable item)  //인벤토리에 아이템 추가
        {
            Items.Add(item);
            switch (item.ItemType)   //아이템 분류 과정
            {
                case ItemType.Equipment:
                    Equipment equip = (Equipment)item;
                    Equipments.Add(equip);
                    if (equip.IsEquipped)   //장착된 아이템인 경우
                    {
                        EquippedItems[(int)equip.EquipType] = equip;
                    }
                    break;
                default:
                    break;
            }
        }

        public void RemoveItem(ITradable item)  //인벤토리에서 아이템 제거
        {
            Items.Remove(item);
            switch (item.ItemType)   //아이템 분류 과정
            {
                case ItemType.Equipment:
                    Equipments.Remove(item);
                    EquippedItems[(int)EquipType.Armor] = null;
                    OnEquipChanged?.Invoke();
                    break;
                default:
                    break;
            }
        }

        //장비, 소비, 기타 아이템을 따로 표기하는 기능은 아직 구현 안됨
        public void ShowItems()     // 2: 인벤토리 창 - 모든 아이템 표시
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("<인벤토리>");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("\n[아이템 목록]");
                foreach (ITradable item in Items)
                {
                    Console.Write("- ");
                    item.ShowInfo(false);
                }
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요.");

                switch (Utils.GetPlayerInput())
                {
                    case 1:
                        ControlEquipments();
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
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

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
                        EquippedItems[equipIndex] = null;
                    }
                    else
                    {
                        //같은 종류 장비가 이미 장착되어 있으면 해제
                        if (EquippedItems[equipIndex] != null)
                        {
                            EquippedItems[equipIndex].UnEquip();
                        }

                        selected.Equip();
                        EquippedItems[equipIndex] = selected;
                    }
                    OnEquipChanged?.Invoke();
                }
            }
        }
    }
}
