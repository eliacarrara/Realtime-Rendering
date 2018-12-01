using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace RealtimeRendering
{
    class Renderer
    {
        const float DPMS = (float)Math.PI / 8 / 1000f; // 22.5° per second
        const float K = 40;
        static readonly Matrix4x4 WORLD_TRANSLATION = Matrix4x4.CreateTranslation(new Vector3(0, 0, 10));
        static readonly float Z_MIN = WORLD_TRANSLATION.M43 - 2f;
        static readonly float Z_MAX = WORLD_TRANSLATION.M43 + 2f;

        readonly byte[] _canvasBuffer;
        readonly Vector3[] _colorBuffer;
        readonly float[] _depthBuffer;
        readonly Vector3[] _posBuffers;
        readonly Vector2[] _tposBuffers;
        readonly Vector3[] _normalBuffers;
        readonly Vector3[] _diffBuffers;

        public Renderer(int width, int height, int stride)
        {
            Width = width;
            Height = height;
            Stride = stride;
            _canvasBuffer = new byte[Height * Stride];

            _colorBuffer = new Vector3[Height * Width];
            _depthBuffer = new float[Height * Width];
            _posBuffers = new Vector3[Height * Width];
            _tposBuffers = new Vector2[Height * Width];
            _normalBuffers = new Vector3[Height * Width];
            _diffBuffers = new Vector3[Height * Width];
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Stride { get; private set; }

        public byte[] Render(Scene s)
        {
            var angle = DPMS * Environment.TickCount;
            var rM = Helper.GetRotationMatrix(angle, angle, 0);
            var matrices = Helper.GetMatricesPair(rM * WORLD_TRANSLATION);
            int i = 0;

            #region Buffer Reset
            for (i = 0; i < _canvasBuffer.Length; i++)
                _canvasBuffer[i] = 0;

            for (i = 0; i < _colorBuffer.Length; i++)
            {
                _colorBuffer[i] = Vector3.Zero;
                _depthBuffer[i] = float.PositiveInfinity;
                _posBuffers[i] = Vector3.Zero;
                _tposBuffers[i] = Vector2.Zero;
                _normalBuffers[i] = Vector3.Zero;
                _diffBuffers[i] = Vector3.Zero;
            }
            #endregion

            for (i = 0; i < s.FaceElement.Length; i++)
            {
                var t = s.FaceElement[i];
                var tT = s.Textures[t.TextureIndex];

                var aW = Vector4.Transform(s.Vertecies[t.A], matrices.Item1);
                var bW = Vector4.Transform(s.Vertecies[t.B], matrices.Item1);
                var cW = Vector4.Transform(s.Vertecies[t.C], matrices.Item1);

                var aN = Vector4.Transform(s.Normals[t.NormalA], matrices.Item2);
                var bN = Vector4.Transform(s.Normals[t.NormalB], matrices.Item2);
                var cN = Vector4.Transform(s.Normals[t.NormalC], matrices.Item2);

                var aT = Helper.ToHomogeneous(s.TexturePoints[t.TextureA], Helper.FromHomogeneous(aW).Z);
                var bT = Helper.ToHomogeneous(s.TexturePoints[t.TextureB], Helper.FromHomogeneous(bW).Z);
                var cT = Helper.ToHomogeneous(s.TexturePoints[t.TextureC], Helper.FromHomogeneous(cW).Z);

                var aS = ToScreenCoordinates(aW);
                var bS = ToScreenCoordinates(bW);
                var cS = ToScreenCoordinates(cW);

                var abS = bS - aS;
                var acS = cS - aS;

                // backface culling - z component of cross product
                if (abS.X * acS.Y - abS.Y * acS.X <= 0)
                    continue;

                var left = Math.Min(aS.X, Math.Min(bS.X, cS.X));
                var right = Math.Max(aS.X, Math.Max(bS.X, cS.X));
                var top = Math.Min(aS.Y, Math.Min(bS.Y, cS.Y));
                var bottom = Math.Max(aS.Y, Math.Max(bS.Y, cS.Y));

                var det = 1.0f / (abS.X * acS.Y - acS.X * abS.Y);
                var aInv = det * acS.Y; var bInv = det * -acS.X;
                var cInv = det * -abS.Y; var dInv = det * abS.X;

                for (int y = (int)top; y <= bottom && y < Height; y++)
                {
                    var yOffset = y * Height;
                    for (int x = (int)left; x <= right && x < Width; x++)
                    {
                        var ap = new Vector2(x, y) - aS;
                        var u = aInv * ap.X + bInv * ap.Y;
                        var v = cInv * ap.X + dInv * ap.Y;

                        // checks if point is in triangle
                        if (u < 0 || v < 0 || u + v > 1)
                            continue;

                        var index = yOffset + x;
                        _posBuffers[index] = Helper.FromHomogeneous(aW + u * (bW - aW) + v * (cW - aW));

                        var z = (_posBuffers[index].Z - Z_MIN) / (Z_MAX - Z_MIN);
                        if (z > _depthBuffer[index])
                            continue;

                        _depthBuffer[index] = z;
                        _tposBuffers[index] = Helper.FromHomogeneous(aT + u * (bT - aT) + v * (cT - aT));
                        _normalBuffers[index] = Helper.FromHomogeneous(aN + u * (bN - aN) + v * (cN - aN));
                        _diffBuffers[index] = tT.GetColor(_tposBuffers[index]);
                    }
                }
            }

            Parallel.For(0, Height, y =>
            {
                for (int x = 0; x < Width; x++)
                {
                    var bufPos = y * Width + x;
                    var outPos = y * Stride + (x * 3);
                    var c = CalcColor(s.Lights, _normalBuffers[bufPos], _posBuffers[bufPos], _diffBuffers[bufPos], s.Camera.Eye);
                    _canvasBuffer[outPos++] = Helper.ToGammaCorrectedByte(c.X);
                    _canvasBuffer[outPos++] = Helper.ToGammaCorrectedByte(c.Y);
                    _canvasBuffer[outPos++] = Helper.ToGammaCorrectedByte(c.Z);
                }
            });

            return _canvasBuffer;
        }

        private Vector3 CalcColor(Light[] lights, Vector3 n, Vector3 p, Vector3 c, Vector3 o)
        {
            n = Vector3.Normalize(n);
            Vector3 diff = Vector3.Zero;
            Vector3 spec = Vector3.Zero;
            for (int i = 0; i < lights.Length; i++)
            {
                var color = lights[i].Color;
                var l = Vector3.Normalize(lights[i].Position - p);
                var cosTheta = Vector3.Dot(n, l);
                var r = 2 * cosTheta * n - l;

                if (cosTheta >= 0)
                {
                    diff += color * cosTheta * c;

                    var alpha = Vector3.Dot(r, Vector3.Normalize(p - o));
                    if (alpha < 0)
                        spec += color * (float)Math.Pow(alpha, K);
                }
            }

            return diff + spec;
        }

        private Vector2 ToScreenCoordinates(Vector4 pW)
        {
            var v = Helper.FromHomogeneous(pW);
            var x = (Width * v.X / v.Z) + (Width / 2.0f);
            var y = (Width * v.Y / v.Z) + (Height / 2.0f);
            return new Vector2(x, y);
        }
    }
}
