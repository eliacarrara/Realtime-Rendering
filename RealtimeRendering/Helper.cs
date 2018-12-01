using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Drawing.Imaging;

namespace RealtimeRendering
{
    class Helper
    {
        private const float GAMMA = 2.2f;
        private const float GAMMA_INV = 1 / GAMMA;

        public static byte ToGammaCorrectedByte(float value)
        {
            float val = (float)Math.Pow(value, GAMMA_INV);
            if (val >= 1) return 255;
            if (val <= 0) return 0;
            return (byte)(val * 255);
        }

        public static float FromSRGBByte(byte value)
        {
            return (float)Math.Pow(value / 255.0f, GAMMA);
        }

        public static byte Clamp8(int value)
        {
            if (value >= 255) return 255;
            if (value <= 0) return 0;
            return (byte)value;
        }

        public static int CalcStride(int width, int bpp)
        {
            var raw = width * bpp / 8;
            return (raw % 4 == 0) ? raw : raw + (4 - raw % 4);
        }

        public static Vector3 ToHomogeneous(Vector2 vector, float value)
        {
            return new Vector3(vector, 1) / value;
        }

        public static Vector2 FromHomogeneous(Vector3 vector)
        {
            Debug.Assert(vector.Z != 0);
            var v = vector / vector.Z;
            return new Vector2(v.X, v.Y);
        }

        public static Vector3 FromHomogeneous(Vector4 vector)
        {
            Debug.Assert(vector.W != 0);
            var v = vector / vector.W;
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Tuple<Matrix4x4, Matrix4x4> GetMatricesPair(Matrix4x4 m)
        {
            if (!Matrix4x4.Invert(m, out Matrix4x4 n))
                throw new ArithmeticException();

            return new Tuple<Matrix4x4, Matrix4x4>(m, Matrix4x4.Transpose(n));
        }

        public static Matrix4x4 GetRotationMatrix(float x, float y, float z)
        {
            return Matrix4x4.CreateRotationX(x) * Matrix4x4.CreateRotationY(y) * Matrix4x4.CreateRotationZ(z);
        }

        public static Vector3[] LoadBitmap(Bitmap bitmap)
        {
            var texture = new Vector3[bitmap.Width * bitmap.Height];
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                bitmap.PixelFormat);
            int length = bitmapData.Stride * bitmapData.Height;
            var data = new byte[bitmapData.Stride * bitmapData.Height];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, data, 0, length);
            bitmap.UnlockBits(bitmapData);

            int j = 0;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int pos = y * bitmapData.Stride + (x * 4);
                    float b = FromSRGBByte(data[pos++]);
                    float g = FromSRGBByte(data[pos++]);
                    float r = FromSRGBByte(data[pos++]);

                    texture[j++] = new Vector3(r, g, b);
                }
            }

            return texture;
        }
    }
}
