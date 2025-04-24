using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum Job    //직업 종류
    {
        Warrior,
        Mage,
        Archer
    }

    public enum Stat  //스탯 종류
    {
        Health,     //체력
        Attack,     //공격력
        Defense,    //방어력
    }

    public class Player : IBattleUnit
    {
        public int MaxLevel { get; private set; }     //만랩
        public int Experience { get; private set; }   //경험치
        public int[] ExpThresholds { get; private set; } //경험치 상한선

        public Job Job { get; private set; }    //직업
        public int Meso { get; private set; }   //메소
        public Inventory Inventory { get; private set; }    //인벤토리
        public event Action? OnPlayerDie; //플레이어 사망 이벤트

        public Dictionary<int, int> monsterKillCounts; // 몬스터 킬 카운트

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
            monsterKillCounts = new Dictionary<int, int>();
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
            CritChance = playerData.CritChance;
            EvadeChance = playerData.EvadeChance;
            Meso = playerData.Meso;
            monsterKillCounts = playerData.MonsterKillCounts;
            FullMP = playerData.FullMP;
            CurrentMP = playerData.CurrentMP;
            InitializeSkills();
        }

        public void LoadDefaultData()
        {
            //플레이어 기본 데이터 파일 읽어오기
            if (!ConfigLoader.TryLoad<PlayerConfig>(@"..\..\..\resources/player_config.json", out var config))
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
            CritChance = defaultData.CritChance;
            EvadeChance = defaultData.EvadeChance;
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
            foreach (Equipment? item in Inventory.EquippedItems.Values)
            {
                if (item != null)
                {
                    switch (item.Stat)
                    {
                        case Stat.Health:
                            BonusFullHP += item.StatValue;
                            break;
                        case Stat.Attack:
                            BonusAttack += item.StatValue;
                            break;
                        case Stat.Defense:
                            BonusDefense += item.StatValue;
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

        public void ShowInventory()
        {
            Inventory.ShowItems();
        }

        public void ShowPlayerInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("<상태 보기>");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine($"\nLv. {Level}");
            Console.WriteLine($"경험치 :  {Experience} / {ExpThresholds[Level - 1]}");
            Console.WriteLine($"{Name} ({Utils.JobDisplayNames[Job]})");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Defense}");
            Console.WriteLine($"치명타율 : {CritChance}");
            Console.WriteLine($"회피율 : {EvadeChance}");
            Console.WriteLine($"체력 : {CurrentHP}/{FullHP}");
            Console.WriteLine($"마나 : {CurrentMP}/{FullMP}");
            Console.WriteLine($"메소 : {Meso} 메소");
            ShowSkillList();
        }

        public void ShowSkillList()
        {
            Console.WriteLine($"\n[스킬]");

            for (int i = 0; i < Skills.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Skills[i].Name} : {Skills[i].Description}");
            }
        }

        private void InitializeSkills()
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
            }
        }

        public PlayerData GetPlayerData()   //플레이어 데이터 추출
        {
            return new PlayerData(Name, Job, Level, MaxLevel, Experience, ExpThresholds, BaseFullHP, CurrentHP,
                BaseAttack, BaseDefense, CritChance, EvadeChance, Meso, monsterKillCounts, FullMP, CurrentMP);
        }
    }
}
