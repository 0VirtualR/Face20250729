using Face.Events;
using Face.Views;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Extensions
{
  public static  class MainWindowEventExtensions
    {
        public static void RegisterMainWindowEvent(this IEventAggregator aggregator,Action<DisplayImg> action)
        {
            aggregator.GetEvent<MainWindowEvent>().Subscribe(action);
        }
        public static void PublishMainWindowEvent(this IEventAggregator aggregator,DisplayImg displayImg)
        {
            aggregator.GetEvent<MainWindowEvent>().Publish(displayImg);
        }
    }
}
