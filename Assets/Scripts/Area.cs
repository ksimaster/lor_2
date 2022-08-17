using Assets.Scripts.Tiles;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class Area
    {
        private readonly TileMap TileMap;

        public string Id { get; }
        public Tile[] Tiles { get; private set; }
        public int Money { get; set; }

        public Area(TileMap tilemap, string id, int money, MapTile[]  mapTiles)
        {
            TileMap = tilemap;
            Id = id;
            Money = money;
            Update(mapTiles);
        }

        public Area CopyWithTileMap(TileMap tilemap)
        {
            return new Area(tilemap, Id, Money, Tiles.Select(x => x.MapTile).ToArray());
        }

        public void Update(MapTile[] tileIndexes) {
            Tiles = tileIndexes.Select(t=>TileMap.GetTileByXY(t.X, t.Y)).ToArray();
        }

        public string GetPlayerId()
        {
            return string.Join("_", Id.Split('_').Take(2));
        }

        public static List<MapTile> FindLargestArea(List<MapTile> tiles)
        {
            var areas = new List<List<string>>();
            foreach (var tile in tiles)
            {
                var id = $"{tile.X}:{tile.Y}";
                var neighbors = tile.Neighborhoods();
                if (areas.Count == 0)
                {
                    areas.Add(new List<string>() { id });
                    continue;
                }

                var connects = new List<List<string>>();
                foreach (var area in areas)
                {
                    foreach (var neib in neighbors)
                    {
                        if (area.IndexOf($"{neib[0]}:{neib[1]}") != -1)
                        {
                            if (connects.IndexOf(area) == -1)
                            {
                                connects.Add(area);
                            }
                        }
                    }
                }

                if (connects.Count == 0) {
                    areas.Add(new List<string>() { id });
                }
                else
                  if (connects.Count == 1)
                {
                    connects[0].Add(id);
                }
                else
                {
                    var newArea = new List<string>() { id };
                    foreach (var connect in connects)
                    {
                        newArea.AddRange(connect);
                    }

                    areas = areas.Where(a => (connects.IndexOf(a) == -1)).ToList();
                    areas.Add(newArea);
                }
            }

            var bestLen = 0;
            var bestArea = new List<string>();
            foreach (var area in areas)
            {
                if (area.Count > bestLen)
                {
                    bestArea = area;
                    bestLen = area.Count;
                }
            }

            var result = new List<MapTile>();
            foreach (var tile in bestArea)
            {
                var parts = tile.Split(':');
                result.Add(new MapTile(int.Parse(parts[0]), int.Parse(parts[1])));
            }

            return result;
        }
    }
}
