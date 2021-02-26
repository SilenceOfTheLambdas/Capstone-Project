using Microsoft.Xna.Framework;

namespace GrimGame.Engine
{
    public class GridPoint
    {
        public readonly int X;
        public readonly int Y;

        public GridPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPoint(float x, float y)
        {
            X = (int) x;
            Y = (int) y;
        }

        public GridPoint(Vector3 v3)
        {
            var (x, y, _) = v3;
            X = (int) x;
            Y = (int) y;
        }
    }
}