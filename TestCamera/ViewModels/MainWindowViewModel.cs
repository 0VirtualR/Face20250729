using Prism.Commands;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TestCamera.Interface;
using WPFMediaKit;
using WPFMediaKit.DirectShow.Controls;

namespace TestCamera.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public string Title { get; set; } = "Title";

        // 暴露命令和图像数据（不包含UI控件）
        public ICommand CaptureCommand { get; }
      
        private BitmapSource captureImage;
        private readonly ICameraService cameraService;

        public BitmapSource CaptureImage
        {
            get { return captureImage; }
            set { captureImage = value;RaisePropertyChanged(); }
        }


        // 通过服务层操作摄像头（推荐）


        public MainWindowViewModel(ICameraService cameraService)
        {

            CaptureCommand = new DelegateCommand(Capture);
            this.cameraService = cameraService;
        }

        private void Capture()
        {
       
            try
            {
                var bitmap = cameraService.CaptureFrame();
                cameraService.SaveImage(bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"拍照失败: {ex.Message}");
            }
        }
      
    }
}
