using System.Windows;
using Example;
using Prism.Ioc;
using TestCamera.Interface;
using TestCamera.Service;
using TestCamera.ViewModels;
using TestCamera.Views;

namespace TestCamera
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICameraService,CameraService>();
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();


        }
    }
}
