using Face.Common;
using Face.Extensions;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Face.ViewModels
{
    public class MainWindowViewModel : BindableBase,IConfigService
    {
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

        }

        public void Configure()
        {
            regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate("IndexView");
        }
    }
}
