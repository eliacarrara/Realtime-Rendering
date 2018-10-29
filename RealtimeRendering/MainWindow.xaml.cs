using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RealtimeRendering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Scene s;
        readonly Renderer r;
        readonly WriteableBitmap bmp;
        readonly Int32Rect rect;
        readonly Stopwatch sw;

        public MainWindow()
        {
            InitializeComponent();
            var width = (int)PaintCanvas.Width;
            var height = (int)PaintCanvas.Height;
            bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);
            rect = new Int32Rect(0, 0, width, height);

            s = SceneFactory.SpinningBox();
            r = new Renderer(width, height, Helper.CalcStride(width, bmp.Format.BitsPerPixel));
            sw = new Stopwatch();

            CompositionTarget.Rendering += Paint;
        }

        private void Paint(object sender, EventArgs e)
        {
            sw.Stop();
            double milliSeconds = sw.Elapsed.TotalMilliseconds;
            LblFPS.Content = string.Format("{0:N2}fps", 1000.0f / milliSeconds);
            sw.Restart();

            bmp.WritePixels(rect, r.Render(s, milliSeconds), r.Stride, 0);
            PaintCanvas.Background = new ImageBrush { ImageSource = bmp };

            s.RotateAll();

            
        }
    }
}
