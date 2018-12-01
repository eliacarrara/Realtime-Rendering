using RealtimeRendering.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RealtimeRendering
{
    class FaceElement
    {
        public FaceElement(int a, int b, int c, int aN, int bN, int cN, int aT, int bT, int cT, int tIndex)
        {
            A = a;
            B = b;
            C = c;

            NormalA = aN;
            NormalB = bN;
            NormalC = cN;

            TextureA = aT;
            TextureB = bT;
            TextureC = cT;

            TextureIndex = tIndex;
        }

        public int A { get; private set; }
        public int B { get; private set; }
        public int C { get; private set; }

        public int NormalA { get; private set; }
        public int NormalB { get; private set; }
        public int NormalC { get; private set; }

        public int TextureA { get; private set; }
        public int TextureB { get; private set; }
        public int TextureC { get; private set; }

        public int TextureIndex { get; private set; }
    }
}
