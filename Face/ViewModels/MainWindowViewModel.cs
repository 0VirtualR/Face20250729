using Face.Common;
using Face.Extensions;
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

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

        }

        public void Configure()
        {
            regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate("IndexView");
        }
    }
}
