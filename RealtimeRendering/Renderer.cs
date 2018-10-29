using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RealtimeRendering
{
    class Renderer
    {
        readonly Vector3 Z_PLANE = new Vector3(0, 0, 1);
        readonly Vector3 Z_PLANE_N = Vector3.UnitZ;

        byte[] _data;

        public Renderer(float width, float height, int stride)
        {
            Width = width;
            Height = height;
            Stride = stride;
            _data = new byte[(int)Height * Stride];
        }

        public float Width { get; set; }
        public float Height { get; set; }
        public int Stride { get; private set; }

        public byte[] Render(Scene s, double elapsedMilliseconds)
        {
            _data = new byte[(int)Height * Stride];
            for (int i = 0; i < s.Triangles.Length; i++)
            {
                Triangle t = s.Triangles[i];
                var pA = Transform2D(s.Points[t.A].Position);
                var pB = Transform2D(s.Points[t.B].Position);
                var pC = Transform2D(s.Points[t.C].Position);
                var ab = pB - pA;
                var ac = pC - pA;

                // backface culling
                if (Vector3.Cross(new Vector3(ab, 0), new Vector3(ac, 0)).Z <= 0) 
                    continue;

                float left = Math.Min(pA.X, Math.Min(pB.X, pC.X));
                float right = Math.Max(pA.X, Math.Max(pB.X, pC.X));
                float top = Math.Min(pA.Y, Math.Min(pB.Y, pC.Y));
                float bottom = Math.Max(pA.Y, Math.Max(pB.Y, pC.Y));

                var a = ab.X; var b = ac.X;
                var c = ab.Y; var d = ac.Y;
                var det = 1.0f / (a * d - b * c);
                var aInv = det * d; var bInv = det * -b;
                var cInv = det * -c; var dInv = det * a;

                for (int y = (int)top; y <= bottom && y < Height; y++)
                {
                    int yOffset = y * Stride;
                    for (int x = (int)left; x <= right && x < Width; x++)
                    {
                        var ap = new Vector2(x, y) - pA;
                        var uv = new Vector2(aInv * ap.X + bInv * ap.Y, cInv * ap.X + dInv * ap.Y);
                        if (uv.X >= 0 && uv.Y >= 0 && uv.X + uv.Y < 1)
                        {
                            var colorA = s.Points[t.A].Color;
                            var colorB = s.Points[t.B].Color;
                            var colorC = s.Points[t.C].Color;
                            var color = colorA + uv.X * (colorB - colorA) + uv.Y * (colorC - colorA);

                            int pos = x * 3 + yOffset;
                            _data[pos++] = Helper.ConvertToGammaCorrectedByte(color.X);
                            _data[pos++] = Helper.ConvertToGammaCorrectedByte(color.Y);
                            _data[pos++] = Helper.ConvertToGammaCorrectedByte(color.Z);
                        }
                    }
                }
            }

            return _data;
        }

        private Vector2 Transform2D(Vector3 point)
        {
            Vector3 v = point + new Vector3(0, 0, 5);
            float x = (Width * v.X / v.Z) + (Width / 2);
            float y = (Width * v.Y / v.Z) + (Height / 2);
            return new Vector2(x, y);
        }
    }
}
