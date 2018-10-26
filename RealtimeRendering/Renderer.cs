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
        public Renderer(float width, float height)
        {
            Width = width;
            Height = height;
        }
        public float Width { get; set; }     
        public float Height { get; set; }     

        public Polygon[] Render(Scene s)
        {
            Polygon[] polygons = new Polygon[s.Triangles.Length];
            for (int i = 0; i < polygons.Length; i++)
            {
                Polygon p = new Polygon();
                Point p1 = VectorToPoint(Transfrom(s.Points[s.Triangles[i].Item1]));
                Point p2 = VectorToPoint(Transfrom(s.Points[s.Triangles[i].Item2]));
                Point p3 = VectorToPoint(Transfrom(s.Points[s.Triangles[i].Item3]));

                p.Points.Add(p1);
                p.Points.Add(p2);
                p.Points.Add(p3);
                p.Stroke = Brushes.Black;
                polygons[i] = p;
            }

            return polygons;
        }

        Vector2 Transfrom(Vector3 point)
        {
            Vector3 vec = new Vector3(0, 0, 5);
            Vector3 v = point + vec;

            float x = (Width * v.X / v.Z) + (Width / 2);
            float y = (Width * v.Y / v.Z) + (Height / 2);

            return new Vector2(x, y);
        }

        static Point VectorToPoint(Vector2 vec)
        {
            return new Point(vec.X, vec.Y);
        }
    }
}
