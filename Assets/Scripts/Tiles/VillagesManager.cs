using System.Collections.Generic;

namespace Assets.Scripts.Village
{
    class VillagesManager
    {
        private List<Village> villages = new List<Village>();

        private readonly int Radius;

        public VillagesManager(int radius = 3)
        {
            Radius = radius;
        }

        public bool CheckVillage(int x, int y)
        {   
            foreach (var village in villages)
            {
                if (!village.CheckRadius(x, y, Radius))
                {
                    return false;
                }
            }

            return true;
        }

        public void AddVillage(int x, int y)
        {
            villages.Add(new Village(x, y));
        }
    }
}
