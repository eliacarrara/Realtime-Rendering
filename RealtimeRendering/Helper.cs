using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RealtimeRendering
{
    class Helper
    {
        private const float GAMMA = 2.2f;
        private const float GAMMA_INV = 1 / GAMMA;

        public static byte ConvertToGammaCorrectedByte(float value)
        {
            float val = (float)Math.Pow(value, GAMMA_INV);
            if (val >= 1)
                return 255;
            if (val <= 0)
                return 0;

            return (byte)(val * 255);
        }

        public static int CalcStride(int width, int bpp)
        {
            var raw = width * bpp / 8;
            return (raw % 4 == 0) ? raw : raw + (4 - raw % 4);
        }

        public static Tuple<float, float, float, float> Invert(float a, float b, float c, float d)
        {
            float det = 1.0f / (a * d - b * c);
            return new Tuple<float, float, float, float>(d * det, -b * det, -c * det, a * det);
        }

    }
}
