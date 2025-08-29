using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using WPFMediaKit.DirectShow.Controls;

namespace TestCamera.Interface
{
    public interface ICameraService
    {
        void SaveImage(BitmapSource image);
        //void StartCamera();
        BitmapSource CaptureFrame();
        VideoCaptureElement GetCameraView(); // 返回可视化控件
    }
}
