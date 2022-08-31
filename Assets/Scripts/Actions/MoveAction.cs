using Assets.Scripts.Tiles;
using UnityEngine.Events;


namespace Assets.Scripts.Actions
{
    public class MoveAction : UnityEvent<MoveActionData> { }

    public class MoveActionData
    {
        public readonly Tile UnitTile;

        public readonly Tile TargetTile;

        public MoveActionData(Tile unitTile, Tile targetTile)
        {
            UnitTile = unitTile;
            TargetTile = targetTile;
        }
    }
}
