using Face.Extensions;
using Face.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.ViewModels
{
  public class IndexViewModel:ViewModelBase
    {
        public IndexViewModel(IRegionManager regionManager,IEventAggregator aggregator):base(regionManager,aggregator)
        {
        }
        public Dictionary<string, object> ToLoginParam { get; } = new Dictionary<string, object>
        {
            {"ViewName","LoginView" },
            {"IsPublish",true }
        };
        public Dictionary<string, object> ToCameraParam { get; } = new Dictionary<string, object>
        {
            {"ViewName","CameraView" }
        };
     
    }
}
