using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        readonly Timer t;
        
        public MainWindow()
        {
            InitializeComponent();
            var width = (int)PaintCanvas.Width;
            var height = (int)PaintCanvas.Height;
            bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);
            rect = new Int32Rect(0, 0, width, height);

            s = SceneFactory.ChessCube();
            r = new Renderer(width, height, Helper.CalcStride(width, bmp.Format.BitsPerPixel));
            sw = new Stopwatch();

            CompositionTarget.Rendering += Paint;
            //MainWindow1.KeyDown += MainWindow1_KeyDown;
            //t = new Timer(switcher, null, 0, 1000);
        }

        private void switcher(object sender)
        {
            switch (r.OutputType)
            {
                case Renderer.Output.FullRender:
                    r.OutputType = Renderer.Output.ZBuffer;
                    break;
                case Renderer.Output.ZBuffer:
                    r.OutputType = Renderer.Output.PositionBuffer;
                    break;
                case Renderer.Output.PositionBuffer:
                    r.OutputType = Renderer.Output.TexturePositionBuffer;
                    break;
                case Renderer.Output.TexturePositionBuffer:
                    r.OutputType = Renderer.Output.NormalBuffer;
                    break;
                case Renderer.Output.NormalBuffer:
                    r.OutputType = Renderer.Output.DiffuseColorBuffer;
                    break;
                case Renderer.Output.DiffuseColorBuffer:
                    r.OutputType = Renderer.Output.FullRender;
                    break;
                default:
                    break;
            }
        }

        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {
            switcher(sender);
        }

        private void Paint(object sender, EventArgs e)
        {
            sw.Stop();
            LblFPS.Content = $"{(1000.0f / sw.Elapsed.TotalMilliseconds):N0}fps - {r.OutputType.ToString()}";
            sw.Restart();

            var data = r.Render(s);
            bmp.WritePixels(rect, data, r.Stride, 0);
            PaintCanvas.Background = new ImageBrush { ImageSource = bmp };
        }
    }
}
