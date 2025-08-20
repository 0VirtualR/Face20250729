using OpenCvSharp;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System;
using System.Reflection.Emit;
using System.Diagnostics.Contracts;

namespace Test4.ViewModels
{
    public class MainWindowViewModel : BindableBase,IDisposable
    {
        private VideoCapture _camera;
        private Mat _frame;
        private DispatcherTimer _frameTimer;



        private WriteableBitmap _writeableBitmap;  // 字段 - 内部工作对象
        public BitmapSource CameraImage { get; private set; }// 属性 - 对外暴露的接口

        #region 通知更新属性
        private int _selectedCameraIndex = 0;
        public int SelectedCameraIndex
        {
            get => _selectedCameraIndex;
            set => SetProperty(ref _selectedCameraIndex, value);
        }
        private string _statusMessage = "就绪";
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
                    SetupFrameTimer();
                }
            }
        }
        #endregion


        public ObservableCollection<int> AvailableCameras { get; } = new ObservableCollection<int>();
        public ObservableCollection<double> AvailableFrameRates { get; } = new ObservableCollection<double>
        {
            15, 30, 60
        };



        public DelegateCommand StartCameraCommand { get; set; }
        public DelegateCommand StopCameraCommand { get; set; }
        public DelegateCommand CaptureCommand { get; set; }
        public DelegateCommand RefreshCamerasCommand { get; set; }

        public MainWindowViewModel()
        {
            InitializeCommands();
            RefreshAvailableCameras();
        }

        private void InitializeCommands()
        {
            StartCameraCommand = new DelegateCommand(StartCamera, () => !IsCameraRunning && AvailableCameras.Count > 0);
            StopCameraCommand = new DelegateCommand(StopCamera, () => IsCameraRunning);
            CaptureCommand = new DelegateCommand(CaptureImage, () => IsCameraRunning);
            RefreshCamerasCommand = new DelegateCommand(RefreshAvailableCameras);
        }

        private void RefreshAvailableCameras()
        {
            AvailableCameras.Clear();
            StatusMessage = "正在检测摄像头...";

            // 检测可用摄像头（0-9）
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using (var testCamera = new VideoCapture(i))
                    {
                        if (testCamera.IsOpened())
                        {
                            AvailableCameras.Add(i);
                        }
                    }
                }
                catch
                {
                    // 忽略无法访问的摄像头
                }
            }

            StatusMessage = AvailableCameras.Count > 0
                ? $"找到 {AvailableCameras.Count} 个摄像头"
                : "未找到可用摄像头";

            RefreshAllCommands();
        }

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

                //Mat _frame 的作用   Mat = OpenCV的图像容器
                //内存管理：自动管理图像数据内存   图像处理：提供丰富的图像操作方法
                //数据载体：存储从摄像头读取的每一帧图像
                _frame = new Mat();

                // 设置摄像头参数
                _camera.FrameWidth = 640;
                _camera.FrameHeight = 480;

                // 创建画布：WriteableBitmap 就像一块数字画布，用于直接操作像素数据
                _writeableBitmap = new WriteableBitmap(
                    _camera.FrameWidth,
                    _camera.FrameHeight,
                    96, 96,
                    PixelFormats.Bgr24,
                    null);

                CameraImage = _writeableBitmap;
                RaisePropertyChanged(nameof(CameraImage));


                /*// 第1步：建立引用关系（只执行一次）
CameraImage = _writeableBitmap; // 现在 CameraImage 指向 _writeableBitmap

// 第2步：更新_writeableBitmap的内容（每秒30次）
_writeableBitmap.WritePixels(...); // 直接修改像素数据

// 第3步：WPF自动检测到变化并渲染
// 不需要再次通知，因为使用的是同一个对象！*/





                // 设置帧率定时器
                SetupFrameTimer();

                IsCameraRunning = true;
                StatusMessage = $"摄像头 {SelectedCameraIndex} 运行中 - {FrameRate}FPS";
                RefreshAllCommands();
            }
            catch (Exception ex)
            {
                StatusMessage = $"启动摄像头失败: {ex.Message}";
            }
        }

        private void SetupFrameTimer()
        {
            //DispatcherTimer = WPF的专用定时器  UI线程安全：在UI线程上触发Tick事件
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
            if (_camera == null || !_camera.IsOpened() || _frame == null)
                return;

            try
            {
                if (!_camera.Read(_frame) || _frame.Empty())
                    return;

                //Lock/Unlock 的作用：线程安全：防止在写入时被其他线程读取
                // 性能优化：批量操作，减少渲染开销  数据一致性：确保完整帧被渲染
                _writeableBitmap.Lock();

                // 确保尺寸匹配
                if (_writeableBitmap.PixelWidth != _frame.Width ||
                    _writeableBitmap.PixelHeight != _frame.Height)
                {
                    _writeableBitmap.Unlock();
                    return;
                }

                // 计算缓冲区大小
                int bufferSize = (int)(_frame.Step() * _frame.Height);
                byte[] buffer = new byte[bufferSize];

                // 从IntPtr复制到byte[]
                System.Runtime.InteropServices.Marshal.Copy(
                    _frame.Data,
                    buffer,
                    0,
                    bufferSize);

                // 使用byte[]写入WriteableBitmap
                var rect = new System.Windows.Int32Rect(0, 0, _frame.Width, _frame.Height);
                _writeableBitmap.WritePixels(
                    rect,
                    buffer,
                    (int)_frame.Step(),
                    0);

                _writeableBitmap.Unlock();
            }
            catch (Exception ex)
            {
                StatusMessage = $"帧处理错误: {ex.Message}";
            }
        }
        private void StopCamera()
        {
            _frameTimer?.Stop();
            _frameTimer = null;

            _camera?.Release();
            _camera?.Dispose();
            _camera = null;

            _frame?.Dispose();
            _frame = null;

            IsCameraRunning = false;
            StatusMessage = "摄像头已停止";
            RefreshAllCommands();
        }

        private void CaptureImage()
        {
            if (_frame == null || _frame.Empty())
                return;

            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filename = $"capture_{timestamp}.jpg";

                // 使用OpenCvSharp保存图像
                Cv2.ImWrite(filename, _frame);

                StatusMessage = $"已保存: {filename}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"保存失败: {ex.Message}";
            }
        }

        private void RefreshAllCommands()
        {
            StartCameraCommand.RaiseCanExecuteChanged();
            StopCameraCommand.RaiseCanExecuteChanged();
            CaptureCommand.RaiseCanExecuteChanged();
            RefreshCamerasCommand.RaiseCanExecuteChanged();
        }

        public void Dispose()
        {
            StopCamera();
        }
    }
}
