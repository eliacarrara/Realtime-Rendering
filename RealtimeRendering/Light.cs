using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering
{
    public class Light
    {
        public Light(Vector3 p, Vector3 c, float intensity)
        {
            Position = p;
            Color = c * intensity;
        }

        public Vector3 Position { get; private set; }
        public Vector3 Color { get; private set; }
    }
}
