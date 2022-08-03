using System;

namespace Assets.Scripts.Village
{
    public class Village: MapTile
    {
        public Village(int x, int y) : base(x, y)
        {
        }

        /// TODO: Fix to a correct one.
        public bool CheckRadius(int x, int y, int R)
        {
            int dx = Math.Abs(x - X);
            int dy = Math.Abs(y - Y);

            if (dx > R || dy > R)
                return true;

            return dx * dx + dy * dy > R * R; 
        }
    }
}
