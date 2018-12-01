using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering
{
    public class Camera
    {
        public Camera(Vector3 up, Vector3 eye)
        {
            Up = Vector3.Normalize(up);
            Eye = eye;
        }

        public Vector3 Up { get; private set; }
        public Vector3 Eye { get; private set; }
    }
}
