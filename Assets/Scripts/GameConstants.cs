using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class GameConstants
    {
        public const int StartingMoney = 20;
        public const int GardenIncome = 3;

        public static readonly UnitType[] UnitTypes = new[] {
            UnitType.EMPTY,
            UnitType.GARDEN,
            UnitType.TOWER,
            UnitType.MILITIA,
            UnitType.SWORDSMAN,
            UnitType.SPEARMAN,
            UnitType.HORSEMAN
        };

        public static readonly Dictionary<UnitType, int> Prices = new Dictionary<UnitType, int> {
            { UnitType.GARDEN, 20 },
            { UnitType.TOWER, 12 },
            { UnitType.MILITIA, 10 },
            { UnitType.SWORDSMAN, 20 },
            { UnitType.SPEARMAN, 20 },
            { UnitType.HORSEMAN, 30 },
            { UnitType.BALISTA, 40 },
        };

        public static readonly Dictionary<UnitType, int> MovableUnits = new Dictionary<UnitType, int> {
            { UnitType.MILITIA, 1 },
            { UnitType.SWORDSMAN, 1 },
            { UnitType.SPEARMAN, 1 },
            { UnitType.HORSEMAN, 3 },
            { UnitType.BALISTA, 1 },
        };

        public static readonly Dictionary<UnitType, int> Salaries = new Dictionary<UnitType, int> {
            { UnitType.GARDEN, 20 },
            { UnitType.TOWER, 3 },
            { UnitType.MILITIA, 5 },
            { UnitType.SWORDSMAN, 10 },
            { UnitType.SPEARMAN, 10 },
            { UnitType.HORSEMAN, 12 },
            { UnitType.BALISTA, 12 },
        };
    }
}
