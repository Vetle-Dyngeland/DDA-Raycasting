using Microsoft.Xna.Framework;

namespace OptimizedRaycasting.Managers
{
    public struct Vector2Int
    {
        public int X, Y;

        #region Constructors
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2Int(Vector2 v)
        {
            X = (int)v.X;
            Y = (int)v.Y;
        }

        public Vector2Int(Vector2Int v)
        {
            X = v.X;
            Y = v.Y;
        }
        #endregion Constructors

        public Vector2 ToVector2()
        {
            return new(X, Y);
        }
    }
}