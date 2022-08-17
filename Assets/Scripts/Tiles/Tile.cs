using UnityEngine;

namespace Assets.Scripts.Tiles
{
    public class Tile: TileState
    {
        public GameObject TileObject { get; }

        public Tile(MapTile mapTile, GameState gameState, GameObject tileObject) : base(mapTile, gameState)
        {
            TileObject = tileObject;
        }
    }
}
