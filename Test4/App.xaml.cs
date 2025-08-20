using Prism.Ioc;
using System.Windows;
using Test4.ViewModels;
using Test4.Views;

namespace Test4
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
            containerRegistry.RegisterForNavigation<MainWindow,MainWindowViewModel>();
        }
    }
}
