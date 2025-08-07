using Face.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.ViewModels
{
   public class LoginViewModel:BindableBase
    {
		private string password;
        private readonly IRegionManager regionManager;

        public string Password
        {
			get { return password; }
			set { password = value;RaisePropertyChanged(); }
		}
	public	DelegateCommand<string> NavigateCommand { get; private set; }
	public	LoginViewModel(IRegionManager regionManager)
		{
			NavigateCommand = new DelegateCommand<string>(Navigate);
            this.regionManager = regionManager;
        }

        private void Navigate(string obj)
        {
            regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate(obj);
        }
    }
}
