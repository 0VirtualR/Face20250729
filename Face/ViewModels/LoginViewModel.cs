using Face.Extensions;
using Face.Interface;
using Face.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
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
   public class LoginViewModel:ViewModelBase
    {
       
        #region 字段属性
        public Dictionary<string, object> ReturnMainViewParam { get; } = new Dictionary<string, object>
        {
            {"IsPublish",true },
            {"IsDisplay",true }
        };
        private string username = "22";
        public string Username
        {
            get => username;
            set =>SetProperty(ref username,value);
        }
        private string password;
        public string Password
        {
            get => password;
			set =>SetProperty(ref  password,value);
		}
        #endregion

        #region 初始化部分
        private readonly ILoginService loginService;
        public DelegateCommand<string> ExecuteCommand { get;}
	public	LoginViewModel(IRegionManager regionManager,IEventAggregator aggregator,ILoginService loginService):base(regionManager,aggregator)
		{
            this.loginService = loginService;
            //ExecuteCommand = new DelegateCommand<string>(Execute);
            ExecuteCommand = new DelegateCommand<string>(async (param)=>await ExecuteAsync(param));
        }
        #endregion

        #region 函数部分

        private async Task ExecuteAsync(string obj)
        {
            switch (obj)
            {
                case "登录":
                    await Login();
                    break;
            }
        }
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return;
            var res = await loginService.Login(Username, Password);
            if (res != null && res.Status)
            {
                Navigate("FaceView");
            }
        }
        #endregion
    }
}
