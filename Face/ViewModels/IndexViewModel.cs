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
  public class IndexViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;

        public DelegateCommand<string> NavigateCommand { get;private set; }
        public IndexViewModel(IRegionManager regionManager,IEventAggregator aggregator)
        {
            NavigateCommand = new DelegateCommand<string>(Navigate);
 
            this.regionManager = regionManager;
            this.aggregator = aggregator;
        }

        private void Navigate(string viewname)
        {
            try
            {
                regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate(viewname, NavigateResult =>
                {
                    if (NavigateResult.Result == true)
                    {
                        if (viewname == "LoginView")
                        {
                            aggregator.PublishMainWindowEvent(new Events.DisplayImg()
                            {
                                IsDisplay = false
                            });
                        }

                    }
                    else
                    {
                        string s = NavigateResult.Error?.Message;
                        string ss = NavigateResult.Error.ToString();
                    }
                });
            }
        catch(Exception ex)
            {
                
            }
        }
    }
}
