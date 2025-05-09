﻿namespace SpartaDungeon
{
    public enum Job    //직업 종류
    {
        Warrior,
        Mage,
        Archer,
        Assassin
    }

    public enum Stat  //스탯 종류
    {
        Health,     //체력
        Attack,     //공격력
        Defense,    //방어력
        CritChance,   //치명타율
        EvadeChance    //회피율
    }

    public class Player : BattleUnit
    {
        public int MaxLevel { get; private set; }     //만랩
        public int Experience { get; private set; }   //경험치
        public int[] ExpThresholds { get; private set; } //경험치 상한선

        public Job Job { get; private set; }    //직업
        public int Meso { get; private set; }   //메소
        public Inventory Inventory { get; private set; }    //인벤토리
        public event Action? OnPlayerDie; //플레이어 사망 이벤트

        public Player(string name, Job job, Inventory inventory)    //새 게임 생성자
        {

            Name = name;
            Job = job;
            Level = 1;
            Experience = 0;
            Inventory = inventory;
            LoadDefaultData();
            inventory.OnEquipChanged += UpdatePlayerStats;
            IsDead = false;
            InitializeSkills();
        }

        public Player(PlayerData playerData, Inventory inventory)   //게임 불러오기 생성자
        {
            Inventory = inventory;
            Name = playerData.Name;
            Job = playerData.Job;
            Level = playerData.Level;
            MaxLevel = playerData.MaxLevel;
            Experience = playerData.Experience;
            ExpThresholds = playerData.ExpThresholds;
            BaseFullHP = playerData.BaseFullHP;
            CurrentHP = playerData.CurrentHP;
            BaseAttack = playerData.BaseAttack;
            BaseDefense = playerData.BaseDefense;
            BaseCritChance = playerData.BaseCritChance;
            BaseEvadeChance = playerData.BaseEvadeChance;
            Meso = playerData.Meso;
            FullMP = playerData.FullMP;
            CurrentMP = playerData.CurrentMP;
            InitializeSkills();
        }

        public void LoadDefaultData()
        {
            //플레이어 기본 데이터 파일 읽어오기
            if (!ConfigLoader.TryLoad<PlayerConfig>(PathConstants.PlayerConfigPath, out var config))
            {
                Console.WriteLine("플레이어 설정을 불러오지 못했습니다.");
                return;
            }
            PlayerData defaultData;
            switch (Job)
            {
                case Job.Warrior:   //전사
                    defaultData = config.BaseWarriorData;
                    break;
                case Job.Mage:      //마법사
                    defaultData = config.BaseMageData;
                    break;
                case Job.Archer:    //궁수
                    defaultData = config.BaseArcherData;
                    break;
                case Job.Assassin:    //궁수
                    defaultData = config.BaseAssassinData;
                    break;
                default:
                    defaultData = config.BaseWarriorData;
                    break;
            }
            MaxLevel = defaultData.MaxLevel;
            ExpThresholds = defaultData.ExpThresholds;
            BaseFullHP = defaultData.BaseFullHP;
            CurrentHP = BaseFullHP;
            BaseAttack = defaultData.BaseAttack;
            BaseDefense = defaultData.BaseDefense;
            BaseCritChance = defaultData.BaseCritChance;
            BaseEvadeChance = defaultData.BaseEvadeChance;
            Meso = defaultData.Meso;
            FullMP = defaultData.FullMP;
            CurrentMP = FullMP;
        }

        public void RestoreAfterLoad()  //게임 불러오기 후 실행
        {
            Inventory.OnEquipChanged += UpdatePlayerStats;
            UpdatePlayerStats();
        }


        public void UpdatePlayerStats()
        {
            BonusFullHP = 0;
            BonusAttack = 0;
            BonusDefense = 0;
            BonusCritChance = 0;
            BonusEvadeChance = 0;
            foreach (Equipment? item in Inventory.EquippedItems.Values)
            {
                if (item != null)
                {
                    switch (item.Stat)
                    {
                        case Stat.Health:
                            BonusFullHP += (int)item.StatValue;
                            break;
                        case Stat.Attack:
                            BonusAttack += (int)item.StatValue;
                            break;
                        case Stat.Defense:
                            BonusDefense += (int)item.StatValue;
                            break;
                        case Stat.CritChance:
                            BonusCritChance += item.StatValue / 100;
                            break;
                        case Stat.EvadeChance:
                            BonusEvadeChance += item.StatValue / 100;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void GetEXP(int exp)   //경험치 획득
        {
            Experience += exp;
            while (Experience >= ExpThresholds[Level - 1])
            {
                if (Level == MaxLevel)      //이미 만랩인 경우
                {
                    Experience = ExpThresholds[Level - 1];
                    return;
                }
                int overflow = Experience - ExpThresholds[Level - 1];   //경험치 초과량
                LevelUP();
                Experience = overflow;
            }
        }

        protected override void LevelUpStats()
        {
            BaseAttack += 1;
            BaseDefense += 1;
        }

        public void LevelUP()   //레벨 업
        {
            Console.Clear();
            Console.WriteLine("\n레벨업!");
            Console.WriteLine($"레벨 {Level++} -> {Level}");
            Utils.Pause(true);
        }

        public void ChangeMeso(int meso)
        {
            Meso += meso;
        }

        private void InitializeSkills() // 스킬 초기화
        {
            switch (Job)
            {
                case Job.Warrior:
                    Skills.Add(new PawerStrike());
                    Skills.Add(new SlashBlust());
                    break;
                case Job.Mage:
                    Skills.Add(new MagicClaw());
                    Skills.Add(new Thunderbolt());
                    break;
                case Job.Archer:
                    Skills.Add(new ArrowBlow());
                    Skills.Add(new ArrowBomb());
                    break;
                case Job.Assassin:
                    Skills.Add(new LuckySeven());
                    Skills.Add(new SavageBlow());
                    break;
            }
        }

        public PlayerData GetPlayerData()   //플레이어 데이터 추출
        {
            return new PlayerData(Name, Job, Level, MaxLevel, Experience, ExpThresholds, BaseFullHP, CurrentHP,
                BaseAttack, BaseDefense, BaseCritChance, BaseEvadeChance, Meso, FullMP, CurrentMP);
        }

        public void ShowPlayerInfo() // 플레이어 정보 확인
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            ColorFont.Write("<상태 보기>\n", Color.DarkYellow);
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine($"\nLv. {Level}");
            Console.WriteLine($"경험치 :  {Experience} / {ExpThresholds[Level - 1]}");
            Console.WriteLine($"{Name} ({Utils.JobDisplayNames[Job]})");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"치명타율 : {CritChance * 100:F2} %");
            Console.WriteLine($"회피율 : {EvadeChance * 100:F2} %");
            Console.WriteLine($"체력 : {CurrentHP}/{FullHP}");
            Console.WriteLine($"마나 : {CurrentMP}/{FullMP}");
            Console.WriteLine($"메소 : {Meso} 메소");
            ShowSkillList();
            ShowEquippedItems();
        }

        public void ShowState() // 상태 창
        {
            while (true)
            {
                Console.Clear();
                ColorFont.Write("<상태 보기>\n", Color.DarkYellow);
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");

                //플레이어 입력 받기
                Console.WriteLine("\n1. 캐릭터 정보 ");
                Console.WriteLine("2. 처치 현황");
                ColorFont.Write("\n0. 나가기\n", Color.Magenta);
                Console.Write("\n원하시는 행동을 입력해주세요.");
                switch (Utils.GetPlayerInput())
                {
                    case 0:
                        return;
                    case 1:
                        ShowPlayerInfo();
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
            ColorFont.Write("<상태 보기>\n", Color.DarkYellow);
            Console.WriteLine("처치 현황이 표시됩니다.");
            Console.WriteLine("\n[처치 현황]");
            foreach (var kv in MonsterDataBase.MonsterKillCount)
            {
                // var info = monsters.FirstOrDefault(m => m.Id == kv.Key); // 키 확인
                // var name = info != null ? info.Name : $"ID:{kv.Key}"; // 이름 확인
                if (kv.Value >= 1)
                {
                    Console.WriteLine($"{MonsterDataBase.MonsterDict[kv.Key].Name} : {kv.Value}마리");
                }
            }
        }

        public void ShowEquippedItems() // 장착 장비 확인
        {
            ColorFont.Write("\n[장비]\n", Color.Blue);
            foreach (var kv in Inventory.EquippedItems)
            {
                if (Inventory.EquippedItems[kv.Key] != null)
                {
                    switch (Inventory.EquippedItems[kv.Key].EquipType)
                    {
                        case EquipType.Weapon:
                            ColorFont.Write("\n[무기]\n", Color.Green);
                            Console.WriteLine($"{Inventory.EquippedItems[kv.Key].Name} | 공격력 : + {Inventory.EquippedItems[kv.Key].StatValue}");
                            break;
                        case EquipType.Armor:
                            ColorFont.Write("\n[방어구]\n", Color.Green);
                            Console.WriteLine($"{Inventory.EquippedItems[kv.Key].Name} | 방어력 : + {Inventory.EquippedItems[kv.Key].StatValue}");
                            break;
                        case EquipType.Head:
                            ColorFont.Write("\n[투구]\n", Color.Green);
                            Console.WriteLine($"{Inventory.EquippedItems[kv.Key].Name} | 방어력 : + {Inventory.EquippedItems[kv.Key].StatValue}");
                            break;
                        case EquipType.Glove:
                            ColorFont.Write("\n[장갑]\n", Color.Green);
                            Console.WriteLine($"{Inventory.EquippedItems[kv.Key].Name} | 치명타율 : + {Inventory.EquippedItems[kv.Key].StatValue}");
                            break;
                        case EquipType.Shoe:
                            ColorFont.Write("\n[신발]\n", Color.Green);
                            Console.WriteLine($"{Inventory.EquippedItems[kv.Key].Name} | 회피율 : + {Inventory.EquippedItems[kv.Key].StatValue}");
                            break;
                        case EquipType.SubWeapon:
                            ColorFont.Write("\n[서브무기]\n", Color.Green);
                            Console.WriteLine($"{Inventory.EquippedItems[kv.Key].Name} | 방어력 : + {Inventory.EquippedItems[kv.Key].StatValue}");
                            break;
                    }
                }
            }
        }

        public void ShowSkillList() // 스킬 리스트 확인
        {
            ColorFont.Write("\n[스킬]\n", Color.Blue);

            for (int i = 0; i < Skills.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Skills[i].Name} : {Skills[i].Description}");
            }
        }
    }
}
