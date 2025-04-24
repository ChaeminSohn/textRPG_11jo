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
    public List<ItemInfo> Equipments { get; set; } = new List<ItemInfo>();  //장비 아이템 리스트
    public List<ItemInfo> Usables { get; set; } = new List<ItemInfo>();  //소비 아이템 리스트
    public List<ItemInfo> Others { get; set; } = new List<ItemInfo>();  //기타 아이템 리스트

    public Dictionary<int, int> ShopItems { get; set; } = new Dictionary<int, int>(); //상점 아이템 목록(ID , 개수)
  }
}
