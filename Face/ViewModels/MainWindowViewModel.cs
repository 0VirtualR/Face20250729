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
        private string displayTime;

        public string DisplayTime
        {
            get { return displayTime; }
            set { displayTime = value;RaisePropertyChanged(); }
        }
        private void UpdateTime()
        {
            DisplayTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        }
        private string _title = "Prism Application";
        private readonly IRegionManager regionManager;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bool isDisplayImg=true;

        public bool IsDisplayImg
        {
            get { return isDisplayImg; }
            set { isDisplayImg = value;RaisePropertyChanged(); }
        }

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

        public void Configure()
        {
            regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate("FaceView");
            //regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate("IndexView");
        }
    }
}
