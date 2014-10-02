namespace Assets.Scripts.Game
{
    using System.Collections.Generic;

    using UnityEngine;

    public class CalculateLayout
    {
        public static List<Rect> Calculate(int r, int c, float w, float h, float ox, float oy, float m)
        {
            List<Rect> rectangles = new List<Rect>(r * c);

            float a = ((-m * (c - 1)) + w) / c;
            float b = ((-m * (r - 1)) + h) / r;

            float am = a + m;
            float bm = b + m;

            float oxa2 = ox;
            float oyb2 = oy;

            for (int j = 0; j < r; ++j)
            {
                for (int i = 0; i < c; ++i)
                {
                    float x = oxa2 + (am * i);
                    float y = oyb2 + (bm * j);

                    rectangles.Add(new Rect(x, y, a, b));
                }
            }

            return rectangles;
        }
    }
}
