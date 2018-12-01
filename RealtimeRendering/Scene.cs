using RealtimeRendering.Textures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RealtimeRendering
{
    class Scene
    {
        public Scene(Camera c, Vector3[] v, FaceElement[] f, Vector3[] n, Vector2[] t, ITexture[] textures, Light[] l)
        {
            Camera = c;
            Vertecies = v.Select(pt => new Vector4(pt, 1)).ToArray() ?? throw new ArgumentNullException();
            FaceElement = f ?? throw new ArgumentNullException();
            Normals = n.Select(pt => new Vector4(pt, 0)).ToArray() ?? throw new ArgumentNullException();
            TexturePoints = t ?? throw new ArgumentException();
            Textures = textures ?? throw new ArgumentException();
            Lights = l ?? throw new ArgumentNullException();
        }
        public Camera Camera { get; private set; }
        public Vector4[] Vertecies { get; private set; }
        public FaceElement[] FaceElement { get; private set; }
        public Vector4[] Normals { get; private set; }
        public Vector2[] TexturePoints { get; private set; }
        public ITexture[] Textures { get; private set; }
        public Light[] Lights { get; private set; }
    }
}
