using System.Collections.Generic;

namespace Assets.Scripts.Tiles
{
    public class MapTile
    {
        public int X { get; }

        public int Y { get; }

        public MapTile(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return X + Y * 1009;
        }

        public override bool Equals(object other)
        {
            return other != null && other is MapTile mapTileOther && X == mapTileOther.X && Y == mapTileOther.Y;
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }

        public List<int[]> Neighborhoods() => Neighborhoods(X, Y);

        public static List<int[]> Neighborhoods(MapTile tile)
        {
            int x = tile.X;
            int y = tile.Y;
            return Neighborhoods(x, y);
        }

        public static List<int[]> Neighborhoods(int x, int y)
        {
            int offset = y % 2 == 1 ? 1 : 0;
            return new List<int[]>
            {
                new int[] { x + offset, y - 1 },
                new int[] { x - 1 + offset, y - 1 },
                new int[] { x - 1, y },
                new int[] { x + 1, y },
                new int[] {x + offset, y + 1 },
                new int[] {x - 1 + offset, y + 1 },
            };
        }
    }
}
