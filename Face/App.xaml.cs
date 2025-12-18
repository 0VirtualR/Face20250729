using Face.Common;
using Face.Common.SerialPorts;
using Face.Interface;
using Face.Service;
using Face.Tools;
using Face.ViewModels;
using Face.ViewModels.Dialog;
using Face.Views;
using Face.Views.Dialog;
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
            containerRegistry.RegisterSingleton<ISerialPortDetector,SerialPortDetector>();
            containerRegistry.RegisterSingleton<ISerialPortService, SerialPortService>();
            //SerialPortDetector detector = new SerialPortDetector();
            //string alcoholPort = detector.DetectAlcoholPort();
            //containerRegistry.RegisterSingleton<ISerialPortService>(container =>
            //{
            //    var detector=container.Resolve<ISerialPortDetector>();
            //    string alcoholPort=detector.DetectAlcoholPort();
            //    if (!string.IsNullOrEmpty(alcoholPort))
            //    {
            //        return new SerialPortService(alcoholPort,115200);
            //    }
            //    return null;
            //});

            containerRegistry.RegisterDialog<AddFaceView, AddFaceViewModel>();

            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.Register<IFaceService, FaceService>();
            containerRegistry.Register<ILoginService,LoginService>();
            containerRegistry.Register<IApiService, ApiService>();

            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<FaceView, FaceViewModel>();
            containerRegistry.RegisterForNavigation<CameraView, CameraViewModel>();
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
