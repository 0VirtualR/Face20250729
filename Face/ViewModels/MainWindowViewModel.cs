using Face.Common;
using Face.Extensions;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Threading;

namespace Face.ViewModels
{
    public class MainWindowViewModel : BindableBase,IConfigService
    {
        #region 字段属性
        private string displayTime;
        public string DisplayTime
        {
            get => displayTime;
            set =>SetProperty(ref displayTime, value);
        }
        private string title = "Prism Application";
        public string Title
        {
            get => title;
            set { SetProperty(ref title, value); }
        }
        private bool isDisplayImg=true;
        public bool IsDisplayImg
        {
            get => isDisplayImg;
            set =>SetProperty(ref isDisplayImg, value);
        }
        #endregion

        private readonly IRegionManager regionManager;
        public MainWindowViewModel(IRegionManager regionManager,IEventAggregator aggregator)
        {
            this.regionManager = regionManager;

            //订阅事件
            aggregator.RegisterMainWindowEvent(arg =>
            {
                IsDisplayImg = arg.IsDisplay;
            });
            //显示时间
            UpdateTime();
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) => UpdateTime();
            timer.Start();
        }

        private void UpdateTime()
        {
            DisplayTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        }
        public void Configure()
        {
            //regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate("FaceView");
            regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate("IndexView");
        }
    }
}
