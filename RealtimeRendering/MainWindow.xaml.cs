using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RealtimeRendering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Renderer r;
        readonly WriteableBitmap bmp;
        readonly Int32Rect rect;
        readonly Stopwatch sw;
        readonly Scene s;

        public MainWindow()
        {
            InitializeComponent();
            var width = (int)PaintCanvas.Width;
            var height = (int)PaintCanvas.Height;
            bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);
            rect = new Int32Rect(0, 0, width, height);

            s = SceneFactory.Box();
            r = new Renderer(width, height, Helper.CalcStride(width, bmp.Format.BitsPerPixel));
            sw = new Stopwatch();

            CompositionTarget.Rendering += Paint;
        }

        private void Paint(object sender, EventArgs e)
        {
            sw.Stop();
            LblFPS.Content = $"{(1000.0f / sw.Elapsed.TotalMilliseconds):N0}fps";
            sw.Restart();

            var data = r.Render(s);
            bmp.WritePixels(rect, data, r.Stride, 0);
            PaintCanvas.Background = new ImageBrush { ImageSource = bmp };
        }
    }
}
