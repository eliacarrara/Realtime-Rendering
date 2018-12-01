using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering.Textures
{
    class GradientColor2D : ITexture
    {
        // a------------b
        // |            |
        // |            |
        // |            |
        // c------------d

        Vector3 a;
        Vector3 b;
        Vector3 c;
        Vector3 d;

        public GradientColor2D(Vector3 colorA, Vector3 colorB, Vector3 colorC, Vector3 colorD)
        {
            a = colorA;
            b = colorB;
            c = colorC;
            d = colorD;
        }

        public Vector3 GetColor(Vector2 vec)
        {
            if (vec.X >= 1.0f) vec.X = 1;
            if (vec.X < 0.0f) vec.X = 0;
            if (vec.Y >= 1.0f) vec.Y = 1;
            if (vec.Y < 0.0f) vec.Y = 0;

            Vector3 ab = Vector3.Lerp(a, b, vec.X);
            Vector3 cd = Vector3.Lerp(c, d, vec.X);
            return Vector3.Lerp(ab, cd, vec.Y);
        }
    }
}
