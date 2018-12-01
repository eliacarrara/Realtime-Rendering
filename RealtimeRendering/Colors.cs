using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering
{
    class Colors
    {
        private static readonly Random rnd = new Random();

        public static readonly Vector3 RED = new Vector3(1, 0, 0);
        public static readonly Vector3 YELLOW = new Vector3(1, 1, 0);
        public static readonly Vector3 GREEN = new Vector3(0, 1, 0);
        public static readonly Vector3 CYAN = new Vector3(0, 1, 1);
        public static readonly Vector3 BLUE = new Vector3(0, 0, 1);
        public static readonly Vector3 MAGENTA = new Vector3(1, 0, 1);
        public static readonly Vector3 PINK = new Vector3(1, 0.2f, 0.86f);
        public static readonly Vector3 ORANGE = new Vector3(1, 0.5f, 0);
        public static readonly Vector3 BLACK = new Vector3(0);
        public static readonly Vector3 WHITE = new Vector3(1);
        public static readonly Vector3 GREY = new Vector3(0.5f);

        public static Vector3 Rnd()
        {
            return FromRgb((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        }

        public static Vector3 FromRgb(float r, float g, float b)
        {
            return new Vector3(r, g, b);
        }
    }
}
