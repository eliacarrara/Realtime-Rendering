using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Shapes;

namespace RealtimeRendering
{
    class Scene
    {
        public Scene(Vector3[] points, Tuple<int, int, int>[] triangles)
        {
            Points = points;
            Triangles = triangles;
        }

        public Vector3[] Points { get; private set; }
        public Tuple<int, int, int>[] Triangles;
        public static Scene Triangle ()
        {
            Vector3[] points = new Vector3[]
            {
                new Vector3(-1, -1, -1), // top
                new Vector3(1, -1, -1),
                new Vector3(1, 1, -1),
                new Vector3(-1, 1, -1),

                new Vector3(-1, -1, 1), // bottom
                new Vector3(1, -1, 1),
                new Vector3(1, 1, 1),
                new Vector3(-1, 1, 1),
            };
            Tuple<int, int, int>[] triangles = new Tuple<int, int, int>[]
            {
                new Tuple<int, int, int>(0, 1, 2), // top
                new Tuple<int, int, int>(0, 2, 3),

                new Tuple<int, int, int>(7, 6, 5), // bottom
                new Tuple<int, int, int>(7, 5, 6),

                new Tuple<int, int, int>(0, 3, 7), // left
                new Tuple<int, int, int>(0, 7, 4),

                new Tuple<int, int, int>(2, 1, 5), // right
                new Tuple<int, int, int>(2, 5, 6),

                new Tuple<int, int, int>(3, 2, 6), // front
                new Tuple<int, int, int>(3, 6, 7),

                new Tuple<int, int, int>(1, 0, 4), // back
                new Tuple<int, int, int>(1, 4, 5),
            };
            return  new Scene(points, triangles);
        }
    }
}
