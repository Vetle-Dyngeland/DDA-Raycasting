using System;

namespace OptimizedRaycasting.Managers
{
    public static class NumberHelper
    {
        public static int GetValue(this float f)
            => f == 0 ? 0 : (int)(MathF.Abs(f) / f);

        public static int GetValue(this int i) => GetValue((float)i);
    }
}