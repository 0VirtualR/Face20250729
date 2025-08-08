using Face.Common;
using Face.Interface;
using Face.Service;
using Face.ViewModels;
using Face.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Net.Http;
using System.Windows;

namespace Face
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App:PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<HttpClient>(() =>
            {
                var client = new HttpClient()
                {
                    //BaseAddress = new System.Uri("http://localhost:7266/"),
                    BaseAddress = new System.Uri(@"http://localhost:7266/"),
                    Timeout = TimeSpan.FromSeconds(30)
                };
                return client;
            });
            containerRegistry.Register<ILoginService,LoginService>();
            containerRegistry.Register<IApiService, ApiService>();

            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
        }
        protected override void OnInitialized()
        {
        
            var service = App.Current.MainWindow.DataContext as IConfigService;
            if(service!=null)
            {
                service.Configure();
            }
            base.OnInitialized();
        }
    }
}
