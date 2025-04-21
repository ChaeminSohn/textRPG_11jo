using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpartaDungeon
{
  internal class ItemConfig
  {
    public List<ItemInfo> Items { get; set; } = new List<ItemInfo>(); //모든 아이템 리스트
    public List<ItemInfo> Equipments { get; set; } = new List<ItemInfo>();  //장비 아이템 리스트
    public List<ItemInfo> Usables { get; set; } = new List<ItemInfo>();  //소비 아이템 리스트
    public List<ItemInfo> Others { get; set; } = new List<ItemInfo>();  //기타 아이템 리스트
  }
}
