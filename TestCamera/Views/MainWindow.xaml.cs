using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WPFMediaKit.DirectShow.Controls;
using System.Linq;
using TestCamera.Service;
using TestCamera.Interface;
using System.Windows.Media.Media3D;
using System.IO;
using System;

namespace TestCamera.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            try
            {
                var devices = MultimediaUtil.VideoInputDevices;
                if (devices.Length == 0)
                {
                    MessageBox.Show("未检测到摄像头设备");
                    return;
                }

                CameraControl.VideoCaptureSource = devices[0].Name;
                CameraControl.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"摄像头初始化失败: {ex.Message}");
            }
        }

        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            if (CameraControl.ActualWidth == 0 || CameraControl.ActualHeight == 0)
            {
                MessageBox.Show("请等待摄像头初始化完成");
                return;
            }

            try
            {
                var bitmap = CaptureFrame();
                SaveImage(bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"拍照失败: {ex.Message}");
            }
        }

        private BitmapSource CaptureFrame()
        {
            // 确保使用Dispatcher保证线程安全
            return Dispatcher.Invoke(() =>
            {
                var bmp = new RenderTargetBitmap(
                    (int)CameraControl.ActualWidth,
                    (int)CameraControl.ActualHeight,
                    96, 96, PixelFormats.Default);

                bmp.Render(CameraControl);
                return bmp;
            });
        }

        private void SaveImage(BitmapSource image)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"Capture_{DateTime.Now:yyyyMMddHHmmss}.png";
            string filePath = Path.Combine(desktopPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
            }

            MessageBox.Show($"图片已保存到:\n{filePath}", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
        }



    }
}
