using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Face.ViewModels
{
    public class CameraViewModel : ViewModelBase,IDisposable
    {
        #region 属性字段
        public ObservableCollection<int> AvailableCameras { get; } =new ObservableCollection<int>();
        public Dictionary<string, object> ReturnMainViewParam { get; } = new Dictionary<string, object>
        {
            {"IsPublish",true}
        };
        private VideoCapture _camera;
        private Mat _frame;
        private DispatcherTimer _frameTimer;
        private WriteableBitmap _writeableBitmap;   //字段 内部工作对象
        public BitmapSource CameraImage { get; private set; } //属性 对外暴露的接口
      
        private int _selectedCameraIndex = 0;
        public int SelectedCameraIndex
        {
            get => _selectedCameraIndex;
            set => SetProperty(ref _selectedCameraIndex, value);
        }
        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private bool _isCameraRunning;
        public bool IsCameraRunning
        {
            get => _isCameraRunning;
            set => SetProperty(ref _isCameraRunning, value);
        }

        private double _frameRate = 30;
        public double FrameRate
        {
            get => _frameRate;
            set
            {
                if (SetProperty(ref _frameRate, value) && IsCameraRunning)
                {
                    /*SetProperty(ref _frameRate, value)：只有值真正改变时返回true
&& IsCameraRunning：并且摄像头正在运行
结果：只有当帧率改变且摄像头运行时，才重新配置定时器*/
                    SetUpFrameTimer();
                }
            }
        }

        #endregion

        #region 初始化

        public CameraViewModel(IRegionManager regionManager, IEventAggregator aggregator) : base(regionManager, aggregator)
        {
            AvailableCameras.Clear();
            StatusMessage = "正在检测摄像头...";
            for(int i = 0; i < 5; i++)
            {
                try
                {
                    using(var testCamera=new VideoCapture(i))
                    {
                        if (testCamera.IsOpened())
                        {
                            AvailableCameras.Add(i);
                        }
                    }
                }
                catch
                {

                }
            }
            StatusMessage = AvailableCameras.Count > 0 ? $"找到{AvailableCameras.Count}个摄像头" : "未找到可用摄像头";

            //打开摄像头
            StartCamera();
        }
        #endregion

        #region 函数实现
        private void StartCamera()
        {
            try
            {
                _camera = new VideoCapture(SelectedCameraIndex);
                if (!_camera.IsOpened())
                {
                    StatusMessage = "无法打开摄像头";
                    return;
                }
                _camera.FrameWidth = 800;
                _camera.FrameHeight = 600;
                _frame = new Mat();
                _writeableBitmap = new WriteableBitmap(
                  _camera.FrameWidth,
                  _camera.FrameHeight,
                  96, 96,
                  PixelFormats.Bgr24,
                  null);
                CameraImage = _writeableBitmap;
                RaisePropertyChanged(nameof(CameraImage));

                //设置定时器
                SetUpFrameTimer();
                IsCameraRunning = true;
                StatusMessage = $"摄像头{SelectedCameraIndex} 运行中 -  {FrameRate}FPS";
            }
            catch (Exception ex)
            {
                StatusMessage = $"摄像头启动失败! 错误详情：{ex.ToString()}";
            }

        }

        private void SetUpFrameTimer()
        {
            _frameTimer?.Stop();
            _frameTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(1000.0 / FrameRate)
            };
            _frameTimer.Tick += OnFrameTimerTick;
            _frameTimer.Start();

        }

        private void OnFrameTimerTick(object sender, EventArgs e)
        {
            if (_camera == null || !_camera.IsOpened() || _frame == null 
                || !_camera.Read(_frame) || _frame.Empty()) return;

            try
            {


                _writeableBitmap.Lock();
                if (_writeableBitmap.PixelWidth != _frame.Width || _writeableBitmap.PixelHeight != _frame.Height)
                {
                    _writeableBitmap.Unlock();
                    return;
                }
                int bufferSize = (int)(_frame.Step() * _frame.Height);
                byte[] buffer = new byte[bufferSize];
                Marshal.Copy(_frame.Data, buffer, 0, bufferSize);
                var rect = new Int32Rect(0, 0, _frame.Width, _frame.Height);
                _writeableBitmap.WritePixels(rect, buffer, (int)_frame.Step(), 0);
                _writeableBitmap.Unlock();
            }
            catch ( Exception ex)
            {

            }

        }
        private Mat _capturedImage;
        private BitmapSource _capturedImage2;
        private WriteableBitmap _capturedWriteableBitmap;
        private void CaptureImage()
        {
            if (_frame == null || _frame.Empty()) return;
            try
            {
                _capturedImage = _frame.Clone();
                _capturedImage2 = _frame.ToBitmapSource();
                _capturedWriteableBitmap = _frame.ToWriteableBitmap();
            }
            catch
            {

            }
        }
        public void Dispose()
        {
            _frameTimer?.Stop();
            _frameTimer = null;

            _camera?.Release();
            _camera?.Dispose();
            _camera = null;

            _frame?.Dispose();
            _frame = null;
        }
        #endregion
    }
}
