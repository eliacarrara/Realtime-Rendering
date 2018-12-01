using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering.Textures
{
    class SingleColor : ITexture
    {
        readonly Vector3 _c;

        public SingleColor(Vector3 color)
        {
            _c = color;
        }

        public Vector3 GetColor(Vector2 vec)
        {
            return _c;
        }
    }
}
