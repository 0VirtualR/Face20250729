using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Test3.ViewModels
{
    public class MainWindowViewModel : BindableBase,IDisposable
    {
        private VideoCapture _camera;
        private bool _isCameraRunning;
        private BitmapSource _cameraImage;
        private Mat _frame;
        private WriteableBitmap _writeableBitmap;

        public bool IsCameraRunning
        {
            get => _isCameraRunning;
            set => SetProperty(ref _isCameraRunning, value);
        }

        public BitmapSource CameraImage
        {
            get => _cameraImage;
            set => SetProperty(ref _cameraImage, value);
        }

        public DelegateCommand StartCameraCommand { get; }
        public DelegateCommand StopCameraCommand { get; }
        public DelegateCommand CaptureCommand { get; }

        public MainWindowViewModel()
        {
            StartCameraCommand = new DelegateCommand(StartCamera, () => !IsCameraRunning);
            StopCameraCommand = new DelegateCommand(StopCamera, () => IsCameraRunning);
            CaptureCommand = new DelegateCommand(CaptureImage, () => IsCameraRunning);

            _frame = new Mat();
        }

        private void StartCamera()
        {
            try
            {
                _camera = new VideoCapture(0); // 0表示默认摄像头
                if (!_camera.IsOpened())
                {
                    // 处理摄像头打开失败
                    return;
                }

                _camera.FrameWidth = 640;
                _camera.FrameHeight = 480;

                IsCameraRunning = true;
                RefreshCommands();

                // 开始摄像头循环
                CompositionTarget.Rendering += UpdateCameraFrame;
            }
            catch (Exception ex)
            {
                // 处理异常
            }
        }

        private void StopCamera()
        {
            CompositionTarget.Rendering -= UpdateCameraFrame;
            _camera?.Release();
            _camera?.Dispose();
            _camera = null;

            IsCameraRunning = false;
            RefreshCommands();
        }

        private void UpdateCameraFrame(object sender, EventArgs e)
        {
            if (_camera == null || !_camera.IsOpened()) return;

            _camera.Read(_frame);
            if (_frame.Empty()) return;

            // 转换Mat到BitmapSource
            CameraImage = BitmapSourceConverter.ToBitmapSource(_frame);
        }

        private void CaptureImage()
        {
            if (_frame != null && !_frame.Empty())
            {
                // 保存图像
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filename = $"capture_{timestamp}.jpg";

                Cv2.ImWrite(filename, _frame);

                // 这里可以添加保存成功后的逻辑
            }
        }

        private void RefreshCommands()
        {
            StartCameraCommand.RaiseCanExecuteChanged();
            StopCameraCommand.RaiseCanExecuteChanged();
            CaptureCommand.RaiseCanExecuteChanged();
        }

        public void Dispose()
        {
            StopCamera();
            _frame?.Dispose();
        }
    }
}
