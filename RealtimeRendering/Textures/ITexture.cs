using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering.Textures
{
    interface ITexture
    {
        Vector3 GetColor(Vector2 vec);
    }
}
