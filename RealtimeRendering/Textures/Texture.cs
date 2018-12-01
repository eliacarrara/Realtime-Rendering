using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeRendering.Textures
{
    class Texture : ITexture
    {
        readonly Vector3[] _texture = null;
        readonly int _width = 0;
        readonly int _height = 0;

        public Texture(Bitmap bitmap)
        {
            _width = bitmap.Width;
            _height = bitmap.Height;
            _texture = Helper.LoadBitmap(bitmap);
        }

        public Vector3 GetColor(Vector2 vec)
        {
            if (vec.X >= 1.0f) vec.X = 1;
            if (vec.X < 0.0f) vec.X = 0;
            if (vec.Y >= 1.0f) vec.Y = 1;
            if (vec.Y < 0.0f) vec.Y = 0;

            float w = vec.X * (_width - 1);
            float h = vec.Y * (_height - 1);

            int h_min = (int)h;
            int h_max = (int)Math.Ceiling(h);
            int w_min = (int)w;
            int w_max = (int)Math.Ceiling(w);

            float s = vec.X - w_min;
            float t = vec.Y - h_min;

            // a--b
            // |  |
            // c--d
            var a = _texture[h_min * _width + w_min];
            var b = _texture[h_min * _width + w_max];
            var c = _texture[h_min * _width + w_min];
            var d = _texture[h_max * _width + w_max];

            Vector3 ab = Vector3.Lerp(a, b, s);
            Vector3 cd = Vector3.Lerp(c, d, s);
            return Vector3.Lerp(ab, cd, t);
        }
    }
}
