using System;
using System.Numerics;
using System.Windows.Media;

namespace RealtimeRendering
{
    class SceneFactory
    {
        public static Scene SpinningBox()
        {
            var points = new ColorPoint[]
            {
                // top
                new ColorPoint(new Vector3(-1, -1, -1), Colors.BLUE), //a 0
                new ColorPoint(new Vector3(1, -1, -1), Colors.MAGENTA), // b 1
                new ColorPoint(new Vector3(-1, 1, -1), Colors.BLACK), // c 2
                new ColorPoint(new Vector3(1, 1, -1), Colors.RED), // d 3

                // bottom
                new ColorPoint(new Vector3(-1, -1, 1), Colors.CYAN), // e 4
                new ColorPoint(new Vector3(1, -1, 1), Colors.WHITE), // f 5
                new ColorPoint(new Vector3(-1, 1, 1), Colors.GREEN), // g 6
                new ColorPoint(new Vector3(1, 1, 1), Colors.YELLOW) // h 7
            };
            var triangles = new Triangle[]
            {
                
                new Triangle(0, 4, 1), // top
                new Triangle(1, 4, 5),

                new Triangle(6, 2, 3), // bottom
                new Triangle(7, 6, 3),

                new Triangle(2, 6, 0), // left
                new Triangle(6, 4, 0),

                new Triangle(1, 5, 7), // right
                new Triangle(1, 7, 3),

                new Triangle(2, 0, 1), // front
                new Triangle(2, 1, 3),

                new Triangle(6, 5, 4), // back
                new Triangle(6, 7, 5),
            };
            return new Scene(points, triangles);
        }
    }
}
