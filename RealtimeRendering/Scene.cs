using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RealtimeRendering
{
    class Scene
    {
        const float ANGLE = 0.01f; // ~0.57°
        static readonly Matrix4x4 rotMX = Matrix4x4.CreateRotationX(ANGLE);
        static readonly Matrix4x4 rotMY = Matrix4x4.CreateRotationY(ANGLE);
        static readonly Matrix4x4 rotMZ = Matrix4x4.CreateRotationZ(2*ANGLE);
        static readonly Matrix4x4 rotM = rotMX * rotMY * rotMZ; 

        public Scene(ColorPoint[] points, Triangle[] triangles)
        {
            Points = points;
            Triangles = triangles;
        }

        public ColorPoint[] Points { get; private set; }
        public Triangle[] Triangles { get; private set; }
        
        public void RotateAll()
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].Position = Vector3.Transform(Points[i].Position, rotM);
            }
        }
    }
}
