using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering
{
    class ColorPoint
    {
        public ColorPoint(Vector3 p, Vector3 c)
        {
            Position = p;
            Color = c;
        }

        public Vector3 Position{ get; set; }
        public Vector3 Color { get; set; }
    }
}
