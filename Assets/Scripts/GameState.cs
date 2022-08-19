using Assets.Scripts.Tiles;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class GameState
    {
        private readonly Dictionary<MapTile, TreasureTileType> treasuresMap;

        private readonly Dictionary<MapTile, UnitType> unitsMap;

        private readonly Dictionary<MapTile, string> areasMap;
        
        private readonly Dictionary<MapTile, int> movesMap;

        public Player ActivePlayer { get; set; }

        public GameState(MapTile[] availableTiles, Dictionary<MapTile, TreasureTileType> treasuresMap)
        {
            this.treasuresMap = treasuresMap;
            unitsMap = new Dictionary<MapTile, UnitType>();
            areasMap = new Dictionary<MapTile, string>();
            movesMap = new Dictionary<MapTile, int>();

            foreach(var tile in availableTiles)
            {
                unitsMap.Add(tile, UnitType.EMPTY);
                areasMap.Add(tile, Tile.NeutralTileKey);
                movesMap.Add(tile, 0);
            }
        }

        public void SetCellState(MapTile tile, UnitType unitType = UnitType.EMPTY, string areaId = Tile.NeutralTileKey, int moves = 0)
        {
            unitsMap[tile] = unitType;
            areasMap[tile] = areaId;
            movesMap[tile] = moves;
        }

        public void AddEmptyTile(MapTile tile)
        {
            SetCellState(tile);
        }

        public void AddUnitTile(MapTile tile, UnitType unit, string areaId)
        {
            SetCellState(tile, unit, areaId);
        }

        public TreasureTileType GetTreasure(MapTile tile)
        {
            return treasuresMap.ContainsKey(tile) ? TreasureTileType.GRASS : treasuresMap[tile];
        }

        public TileState GetTileState(MapTile tile)
        {
            return new TileState(tile, this);
        }

        public UnitType GetCellUnit(MapTile tile) {
            return unitsMap[tile];
        }

        public void SetCellUnit(MapTile tile, UnitType unit) {
            unitsMap[tile] = unit;
        }

        public string GetCellArea(MapTile tile) {
            return areasMap[tile];
        }

        public void SetCellArea(MapTile tile, string areaId) {
            areasMap[tile] = areaId;
        }

        public int GetCellMoves(MapTile tile) {
            return movesMap[tile];
        }

        public void SetCellMoves(MapTile tile, int moves) {
            movesMap[tile] = moves;
        }

        public Dictionary<string, List<MapTile>> GetAreaDict() {
            var result = new Dictionary<string, List<MapTile>>();
            foreach(var kvpair in areasMap)
            {
                var tile = kvpair.Key;
                var areaId = kvpair.Value;
                if (!result.ContainsKey(areaId))
                {
                    result[areaId] = new List<MapTile>();
                }

                result[areaId].Add(tile);
            }

            return result;
        }

        public void MergeAreas(HashSet<string> mergeAreas, string targetAreaId)
        {
            foreach (var kvpair in areasMap)
            {
                var tile = kvpair.Key;
                var areaId = kvpair.Value;

                if (mergeAreas.Contains(areaId))
                {
                    areasMap[tile] = targetAreaId;
                }
            }
        }
    }
}