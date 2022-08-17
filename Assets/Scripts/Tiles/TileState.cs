using System.Linq;

namespace Assets.Scripts.Tiles
{
    public class TileState
    {
        public const string NeutralTileKey = "N";

        public MapTile MapTile { get; }

        protected GameState gameState;

        public TileState(MapTile mapTile, GameState gameState)
        {
            MapTile = mapTile;
            this.gameState = gameState;
        }

        public void SetAreaId(string id) {
            gameState.SetCellArea(MapTile, id);
        }

        public string GetAreaId() {
            return gameState.GetCellArea(MapTile);
        }

        public UnitType GetUnitType() {
            return gameState.GetCellUnit(MapTile);
        }

        public void SetUnitType(UnitType unit) {
            gameState.SetCellUnit(MapTile, unit);
        }

        public bool isMilitia()
        {
            return GetUnitType() == UnitType.MILITIA;
        }

        public int GetMoves() {
            return gameState.GetCellMoves(MapTile);
        }

        public void SetMoves(int moves) {
            gameState.SetCellMoves(MapTile, moves);
        }

        public string GetPlayerId()
        {
            var parts = GetAreaId().Split('_');
            if (parts.Length < 3)
            {
                return null;
            }

            return string.Join("_", parts.Take(2));
        }

        public static string AreaKey(string playerId, int areaNum) {
            if (playerId == null) {
              return NeutralTileKey;
            }

            return $"player_{playerId}_{ areaNum}";
        }

        public void Activate()
        {
            var unitType = GetUnitType();
            if (GameConstants.MovableUnits.ContainsKey(unitType))
            {
                SetMoves(GameConstants.MovableUnits[unitType]);
            }
            else
            {
                SetMoves(0);
            }
        }
    }
}
