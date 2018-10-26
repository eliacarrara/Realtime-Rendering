using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtimeRendering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Scene s;
        readonly Renderer r;
        public MainWindow()
        {
            InitializeComponent();
            s = Scene.Triangle();
            r = new Renderer((float)PaintCanvas.Width, (float)PaintCanvas.Height);
            CompositionTarget.Rendering += Paint;
        }

        private void Paint(object sender, EventArgs e)
        {
            PaintCanvas.Children.Clear();
            //r.Width = (float)PaintCanvas.Width;
            //r.Height = (float)PaintCanvas.Height;
            var polygons = r.Render(s);

            foreach (var p in polygons)
            {
                PaintCanvas.Children.Add(p);
            }
        }

        Point Transform2D(Vector3 p3D)
        {
            Vector3 vec = new Vector3(0, 0, 5);
            Vector3 v = p3D + vec;

            float x = (float)((PaintCanvas.Width * v.X / v.Z) + (PaintCanvas.Width / 2));
            float y = (float)((PaintCanvas.Width * v.Y / v.Z) + (PaintCanvas.Height / 2));

            return new Point(x, y);
        }
    }
}
