using Face.Common;
using Face.ViewModels;
using Face.Views;
using Prism.DryIoc;
using Prism.Ioc;
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
