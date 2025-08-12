using Face.Extensions;
using Face.Interface;
using Face.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.ViewModels
{
   public class LoginViewModel:BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly ILoginService loginService;
        private string username="22";

        public string Username
        {
            get { return username; }
            set { username = value; RaisePropertyChanged(); }
        }


        private string password;
        public string Password
        {
			get { return password; }
			set { password = value;RaisePropertyChanged(); }
		}
        public DelegateCommand<string> ExecuteCommand { get; private set; }
	public	DelegateCommand<string> NavigateCommand { get; private set; }
	public	LoginViewModel(IRegionManager regionManager,ILoginService loginService)
		{
			NavigateCommand = new DelegateCommand<string>(Navigate);
            this.regionManager = regionManager;
            this.loginService = loginService;
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "登录":Login();break;
            }
        }

        private async Task Login()
        {
            //if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            //    return;
            //var res =await loginService.Login(Username, Password);
            //if(res!=null&& res.Status)
            {

                Navigate("FaceView");


            }
        }

        private void Navigate(string obj)
        {
            try
            {
                regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate(obj);
            }
           catch(Exception ex)
            {

            }
        }
    }
}
