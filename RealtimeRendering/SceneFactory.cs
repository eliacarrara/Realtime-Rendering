using RealtimeRendering.Textures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Media;

namespace RealtimeRendering
{
    class SceneFactory
    {
        public static Scene Box()
        {
            var points = new Vector3[]
            {
                // near
                new Vector3(-1, -1, -1), //a 0 ltn
                new Vector3(1, -1, -1), // b 1 rtn
                new Vector3(-1, 1, -1), // c 2 lbn
                new Vector3(1, 1, -1), // d 3 rbn

                // far
                new Vector3(-1, -1, 1), // e 4 ltf
                new Vector3(1, -1, 1), // f 5 rtf
                new Vector3(-1, 1, 1), // g 6 lbf
                new Vector3(1, 1, 1), // h 7 rbf
            };
            var normals = new Vector3[] {
                 -Vector3.UnitY, // up
                 Vector3.UnitY, // down
                 -Vector3.UnitX, // left
                 Vector3.UnitX, // right
                 -Vector3.UnitZ, // back
                 Vector3.UnitZ, // front
            };
            var tPoints = new Vector2[]
            {
                Vector2.Zero,
                Vector2.UnitX,
                Vector2.UnitY,
                Vector2.One,
            };
            var triangles = new FaceElement[]
            {
                new FaceElement(0, 4, 1, 0, 0, 0, 2, 0, 3, 6), // top
                new FaceElement(1, 4, 5, 0, 0, 0, 3, 0, 1, 6),

                new FaceElement(6, 2, 3, 1, 1, 1, 2, 0, 1, 6), // bottom
                new FaceElement(7, 6, 3, 1, 1, 1, 3, 2, 1, 6),

                new FaceElement(2, 6, 0, 2, 2, 2, 3, 2, 1, 6), // left
                new FaceElement(6, 4, 0, 2, 2, 2, 2, 0, 1, 6),

                new FaceElement(1, 5, 7, 3, 3, 3, 0, 1, 3, 6), // right
                new FaceElement(1, 7, 3, 3, 3, 3, 0, 3, 2, 6),

                new FaceElement(2, 0, 1, 4, 4, 4, 2, 0, 1, 6), // back
                new FaceElement(2, 1, 3, 4, 4, 4, 2, 1, 3, 6),

                new FaceElement(6, 5, 4, 5, 5, 5, 3, 0, 1, 6), // front
                new FaceElement(6, 7, 5, 5, 5, 5, 3, 2, 0, 6),
            };
            var textures = new ITexture[]
            {
                new GradientColor2D(Colors.CYAN, Colors.WHITE, Colors.BLUE, Colors.MAGENTA), // top
                new GradientColor2D(Colors.BLACK, Colors.RED, Colors.GREEN, Colors.YELLOW), // bottom
                new GradientColor2D(Colors.CYAN, Colors.BLUE, Colors.GREEN, Colors.BLACK), // left
                new GradientColor2D(Colors.MAGENTA, Colors.WHITE, Colors.RED, Colors.YELLOW), // right
                new GradientColor2D(Colors.BLUE, Colors.MAGENTA, Colors.BLACK, Colors.RED), // back
                new GradientColor2D(Colors.WHITE, Colors.CYAN, Colors.YELLOW, Colors.GREEN), // front
                new Texture(Properties.Resources.chessboard),
            };
            var lights = new Light[]
            {
                new Light(new Vector3(0, 0, 0), Colors.WHITE, .5f),
            };
            var c = new Camera(-Vector3.UnitY, Vector3.Zero);
            return new Scene(c, points, triangles, normals, tPoints, textures, lights);
        }
    }
}
