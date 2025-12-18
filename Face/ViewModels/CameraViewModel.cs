using Face.Common.SerialPorts;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Prism;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Face.ViewModels
{
    public class CameraViewModel : ViewModelBase,IDisposable,IActiveAware
    {

        #region 酒测流程部分 procedure


        private int _selectPageIndex;
        public int SelectPageIndex
        {
            get => _selectPageIndex;
            set => SetProperty(ref _selectPageIndex, value);
        }
        #endregion

        #region IActiveAware 接口实现
        private bool _isActive;

        public event EventHandler IsActiveChanged;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnIsActiveChanged();
                }
            }
        }
        private void OnIsActiveChanged()
        {  
            //切换为抓拍界面
                    SelectPageIndex = 0;
            if (IsActive)
            {
                //serialPortService.Open();
                // 激活时打开串口并启动定时器
                if (serialPortService.IsOpen)
                {
                    _portTimer.Start();

                    //视频打开
                    _frameTimer?.Start();
                   
                    //serialPortService.DataReceived += OnProcessData;

                }
            }
            else
            {
                // 失活时停止定时器并关闭串口
                //_portTimer.Stop();


                _frameTimer?.Stop();

                //serialPortService.DataReceived -= OnProcessData;

                //serialPortService.Close();
            }

            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }


        #endregion

        #region 属性字段
        private readonly System.Timers.Timer _portTimer;
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

        public CameraViewModel(IRegionManager regionManager, IEventAggregator aggregator,ISerialPortService serialPortService) : base(regionManager, aggregator)
        {
            InitCamera();
            this.serialPortService = serialPortService;
            serialPortService.Open();
            serialPortService.DataReceived += OnProcessData;
            //定时往酒精串口发送数据
            _portTimer = new System.Timers.Timer();
            _portTimer.Interval = 250; // 250ms
            _portTimer.Elapsed += async (s, e) =>
            {
                    await serialPortService.SendAsync("7E013000010031");
            };
            //_portTimer.Start();



        }
        #endregion

        #region 酒测吹气数据处理部分
        private bool _isFirst = true;
        private string _startTime;

        private int _alcoResult;
        public int AlcoResult
        {
            get => _alcoResult;
            set => SetProperty(ref _alcoResult, value);
        }

        private void OnProcessData(string obj)
        {
            Application.Current.Dispatcher.Invoke(()=>ProcessData(obj));
        }

        private void ProcessData(string data)
        {
            if (string.IsNullOrEmpty(data) || data.Length <= 15)
                return;

            // 这里是你原来的业务逻辑
            switch (data)
            {
                case "7e01320002010035":
                    StatusMessage = "等待测试";
                    break;
                case "7e01320002030037":
                    StatusMessage = "允许吹气";
                  
                    break;
                case "7e01320002030138":
                    StatusMessage = "正在吹气";
                    if (_isFirst)
                    {
                        _isFirst = false;
                        _startTime = DateTime.Now.ToString("HH:mm:ss:fff");
                     
                    }
                    break;
                case "7e01320002040038":
                    StatusMessage = "吹气成功";
                    break;
                case "7e01320002020036":
                    StatusMessage = "测试失败";
                    break;
            }

            if (data.StartsWith("7e01340002"))
            {
                ProcessTestResult(data);
            }
        }
        private void ProcessTestResult(string data)
        {
            
            _isFirst = true;


            // 解析结果（和你原来的逻辑一样）
            string str1 = data.Substring(10, 2);
            string str2 = data.Substring(12, 2);
            str1 = Convert.ToInt32(str1, 16).ToString();
            str2 = Convert.ToInt32(str2, 16).ToString();

            if (str1 != "0")
            {
                AlcoResult = int.Parse(str1) + int.Parse(str2);
            }
            StatusMessage = "测试完成";
            SelectPageIndex = 1;
            // 触发后续操作
            //TriggerPhotoCapture();

            // 失活时停止定时器并关闭串口
            _portTimer.Stop();
        }
        #endregion

      

        #region 函数实现

        private void InitCamera()
        {
            AvailableCameras.Clear();
            StatusMessage = "正在检测摄像头...";
            for (int i = 0; i < 5; i++)
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

                }
            }
            StatusMessage = AvailableCameras.Count > 0 ? $"找到{AvailableCameras.Count}个摄像头" : "未找到可用摄像头";

            //打开摄像头
            StartCamera();
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
            //_frameTimer.Start();

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
        private readonly ISerialPortService serialPortService;


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

            _portTimer?.Stop();
        }

        #endregion
    }
}
