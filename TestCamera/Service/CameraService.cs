using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using TestCamera.Interface;
using WPFMediaKit.DirectShow.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Prism.Mvvm;
using System.IO;
using System.Windows.Threading;

namespace TestCamera.Service
{
    public class CameraService : BindableBase, ICameraService
    {
        private readonly VideoCaptureElement CameraControl;

      public  CameraService()
        {
            if(CameraControl== null)
            {
                CameraControl = new VideoCaptureElement();
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
         
        }

        public VideoCaptureElement GetCameraView()
        {


            return CameraControl;
        }

        public BitmapSource CaptureFrame()
        {
            return Application.Current.Dispatcher.Invoke<BitmapSource>(() =>
            {
                var bmp = new RenderTargetBitmap(
                    (int)CameraControl.ActualWidth,
                    (int)CameraControl.ActualHeight,
                    96, 96, PixelFormats.Default);

                bmp.Render(CameraControl);
                return bmp;
            });
        }
        //private BitmapSource CaptureFrame()
        //{
        //    // 确保使用Dispatcher保证线程安全
        //    return Dispatcher.Invoke(() =>
        //    {
        //        var bmp = new RenderTargetBitmap(
        //            (int)CameraControl.ActualWidth,
        //            (int)CameraControl.ActualHeight,
        //            96, 96, PixelFormats.Default);

        //        bmp.Render(CameraControl);
        //        return bmp;
        //    });
        //}

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

        void ICameraService.SaveImage(BitmapSource image)
        {
            SaveImage(image);
        }
    }
}
