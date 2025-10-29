using Face.Extensions;
using Prism.Events;
using System.Windows;

namespace Face.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow(IEventAggregator aggregator)
        {
            InitializeComponent();
            aggregator.RegisterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg.Message);
            },"Login");
        }
    }
}
