using Assets.Scripts.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    class Player
    {
        public int Color { get; }
        private int Id { get; }

        private TileMap TileMap { get; }

        private List<Area> Areas { get; set; }

        public Player(int color, int id, TileMap tilemap, int money = GameConstants.StartingMoney)
        {
            Color = color;
            Id = id;
            TileMap = tilemap;
            var startingAreaId = $"player_{id}_{0}";
            Areas = new List<Area>() {
                new Area(TileMap, startingAreaId, money, new Tiles.MapTile[0])
            };
        }

        public Player CopyWithTileMap(TileMap tileMap)
        {
            var copyPlayer = new Player(Color, Id, tileMap);
            copyPlayer.Areas = Areas.Select(a => a.CopyWithTileMap(tileMap)).ToList();
            return copyPlayer;
        }

        public string GetPlayerId()
        {
            return $"player_{Id}";
        }

        public bool isPlayerArea(string areaId)
        {
            var parts = areaId.Split('_');
            if (parts.Length == 3)
            {
                var playerId = int.Parse(parts[1]);
                return Id == playerId;
            }

            return false;
        }

        public List<Area> GetAreas(bool withUpdate = false)
        {
            if (withUpdate)
            {
                UpdateAreas();
            }
            return Areas;
        }

        public Dictionary<string, List<MapTile>> GetAreaDict()
        {
            return TileMap.GameState.GetAreaDict();
        }

        public void UpdateAreas()
        {
            var areaDict = GetAreaDict();
            var myAreas = new Dictionary<string, List<MapTile>>();
            var areas_to_remove = new List<Area>();

            foreach (var key in areaDict.Keys)
            {
                if (areaDict.ContainsKey(key) && key.StartsWith(GetPlayerId() + '_'))
                {
                    myAreas[key] = areaDict[key];
                }
            }

            foreach (var area in Areas)
            {
                if (areaDict.ContainsKey(area.Id))
                {
                    area.Update(areaDict[area.Id].ToArray());
                }
                else
                {
                    areas_to_remove.Add(area);
                }
            }

            foreach (var area in areas_to_remove)
            {
                Areas.Remove(area);
            }
        }

        public void CollectResources()
        {
            var areas = Areas;
            foreach (var area in areas)
            {
                if (area.Tiles.Length <= 1)
                {
                    area.Money = 0;
                    continue;
                }

                var income = 0;
                foreach (var tile in area.Tiles)
                {
                    income += tile.GetUnitType() == UnitType.GARDEN ? (GameConstants.GardenIncome + 1) : 1;
                }
                area.Money += income;
            }
        }


        public void paySalaries()
        {
            foreach (var area in Areas)
            {
                var pays = 0;
                foreach (var tile in area.Tiles)
                {
                    GameConstants.Salaries.TryGetValue(tile.GetUnitType(), out var salary);
                    if (salary != 0)
                    {
                        pays += salary;
                    }
                }

                area.Money -= pays;
            }
        }

        public void killUnpaid()
        {
            foreach (var area in Areas)
            {
                if (area.Money < 0)
                {
                    foreach (var tile in area.Tiles)
                    {
                        GameConstants.Salaries.TryGetValue(tile.GetUnitType(), out var salary);

                        if (salary != 0 && tile.GetUnitType() != UnitType.TOWER)
                        {
                            tile.SetUnitType(UnitType.EMPTY);
                        }
                    }

                    area.Money = 0;
                }
            }
        }

        public void ActivateUnits()
        {
            Areas.ForEach(area =>
            {
                foreach (var tile in area.Tiles)
                {
                    tile.Activate();
                }
            });
        }


        public void mergeAreas(HashSet<string> mergeAreas, string targetAreaId)
        {
            TileMap.GameState.MergeAreas(mergeAreas, targetAreaId);

            var tempAreaArray = new List<Area>(Areas);

            var targetArea = tempAreaArray.Where(x => x.Id == targetAreaId).First();

            foreach (var mergeAreaId in mergeAreas)
            {
                if (mergeAreaId == targetAreaId)
                {
                    continue;
                }

                var mergeArea = tempAreaArray.Where(x => x.Id == mergeAreaId).First();
                targetArea.Money = targetArea.Money + mergeArea.Money;
                Areas.Remove(mergeArea);
            }

            UpdateAreas();
        }


        public void SplitAreas(List<List<int>> splits, List<HashSet<Tile>> tileSets, string sourceAreaId)
        {
            if (splits.Count <= 1)
            {
                UpdateAreas();
                return;
            }

            var part_sizes = splits.Select(set_id => tileSets[set_id[0]].Count).ToArray();

            var start_id = 0;
            var tempAreaArray = new List<Area>(Areas);

            var target_area = tempAreaArray.Where(x => x.Id == sourceAreaId).FirstOrDefault();
            if (target_area != null)
            {
                UpdateAreas();
                return;
            }

            foreach (var area in tempAreaArray)
            {
                var areaParts = area.Id.Split('_');
                var number_id = int.Parse(areaParts[areaParts.Length - 1]);
                if (number_id > start_id)
                {
                    start_id = number_id;
                }
            }

            var cum_size = 0;
            var cum_money = 0;
            var money_parts = new List<int>();
            foreach (var part_size in part_sizes)
            {
                if (part_size > 1)
                {
                    cum_size += part_size;
                }
            }

            foreach (var part_size in part_sizes)
            {
                if (part_size > 1)
                {
                    var money_part = (int)((part_size * target_area.Money) / cum_size);
                    money_parts.Add(money_part);
                    cum_money += money_part;
                }
                else
                {
                    money_parts.Add(0);
                }
            }

            money_parts[0] += part_sizes[0] > 1 ? (target_area.Money - cum_money) : 0;

            var cntr = start_id + 1;
            var i = -1;
            foreach (var split_group in splits)
            {
                i++;
                if (i == 0)
                {
                    target_area.Money = money_parts[0];
                    continue;
                }

                var new_area_id = $"{GetPlayerId()}_{cntr}";
                cntr += 1;

                var tile_set = tileSets[split_group[0]];
                foreach (var t in tile_set)
                {
                    t.SetAreaId(new_area_id);


                    Areas.Add(new Area(TileMap, new_area_id, money_parts[i], tile_set.ToList().Select(t => t.MapTile).ToArray()));
                }

                UpdateAreas();
            }
        }
    }
}
