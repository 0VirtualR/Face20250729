using Face.Common;
using Face.Interface;
using Face.Service;
using Face.ViewModels;
using Face.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            //https
            //var handler = new HttpClientHandler
            //{
            //    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            //};


            containerRegistry.RegisterSingleton<HttpClient>(() =>
            {
                var client = new HttpClient()
                {
                    //BaseAddress = new System.Uri("http://localhost:3389/"),
                    BaseAddress = new System.Uri("https://localhost:44367/"),
                    //BaseAddress = new System.Uri("http://localhost:44367/"),
                    Timeout = TimeSpan.FromSeconds(30)
                };
                //// 明确协议和头  https
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(
                //    new MediaTypeWithQualityHeaderValue("application/json"));




                return client;
            });


            containerRegistry.Register<IFaceService, FaceService>();
            containerRegistry.Register<ILoginService,LoginService>();
            containerRegistry.Register<IApiService, ApiService>();

            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<FaceView, FaceViewModel>();
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
