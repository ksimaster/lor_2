using System.Collections.Generic;

namespace Assets.Scripts.Tiles
{
    class BuildingsManager
    {
        private readonly List<Village> Villages = new List<Village>();

        private readonly List<Windmill> Windmills = new List<Windmill>();

        private readonly List<SmallForest> SmallForests = new List<SmallForest>();

        private readonly int Radius;

        private readonly int ForestRadius;

        public BuildingsManager(int radius = 3, int forestRadius = 2)
        {
            Radius = radius;
            ForestRadius = forestRadius;
        }

        public bool CheckVillage(int x, int y)
        {   
            foreach (var village in Villages)
            {
                if (!village.CheckRadius(x, y, Radius))
                {
                    return false;
                }
            }

            return true;
        }

        // Use after CheckVillages.
        public bool CheckWindmill(int x, int y)
        {
            foreach (var windmill in Windmills)
            {
                if (!windmill.CheckRadius(x, y, Radius))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckForest(int x, int y)
        {
            foreach (var forest in SmallForests)
            {
                if (forest.CheckRadius(x, y, ForestRadius))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddVillage(int x, int y)
        {
            Villages.Add(new Village(x, y));
        }

        public void AddWindmill(int x, int y)
        {
            Windmills.Add(new Windmill(x, y));
        }

        public void AddSmallForest(int x, int y)
        {
            SmallForests.Add(new SmallForest(x, y));
        }
    }
}
