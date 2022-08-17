using Assets.Scripts.Tiles;

namespace Assets.Scripts.Actions
{
    public class BuildAction
    {

    }

    public class BuildActionData
    {
        public Tile unitTile;
        public string sourceAreaId;
        public UnitType unit;
        public int price;

        public BuildActionData(Tile unitTile, UnitType unit, string sourceAreaId)
        {
            this.unitTile = unitTile;
            this.unit = unit;
            this.sourceAreaId = sourceAreaId;
            var isYouMilitiaTile = unitTile.isMilitia() && unitTile.GetAreaId() == sourceAreaId;
            this.price = GameConstants.Prices[unit] - (isYouMilitiaTile ? GameConstants.Prices[UnitType.MILITIA] : 0);
        }

        public override string ToString()
        {
            return this.unitTile.MapTile.Y + ':' + this.unitTile.MapTile.X + ' ' + this.sourceAreaId + ' ' + this.unit;
        }
}
}
