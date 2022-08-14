using Assets.Scripts.Tiles;
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
    }
}
