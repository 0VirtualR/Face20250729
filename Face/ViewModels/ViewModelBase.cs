using Face.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Face.ViewModels
{
    public abstract class ViewModelBase:BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator aggregator;

        public ICommand NavigateCommand { get; set; }

       public ViewModelBase(IRegionManager regionManager,IEventAggregator aggregator)
        {
            this.regionManager = regionManager;
            this.aggregator = aggregator;
            NavigateCommand = new DelegateCommand<object>(Navigate);
        }

        protected virtual void Navigate(object param)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ViewName","IndexView" },
                {"IsDisplay",false },
                {"IsPublish",false }

            };
            if(param is Dictionary<string,object> inputParam)
            {
                foreach(var key in inputParam)
                {
                    parameters[key.Key] = key.Value;
                }
            }
            string viewName = parameters["ViewName"].ToString();
            bool isDisplay = Convert.ToBoolean(parameters["IsDisplay"]);
            bool isPublish = Convert.ToBoolean(parameters["IsPublish"]);

            if (isPublish)      
            {
                aggregator.PublishMainWindowEvent(new Events.DisplayImg()
                {
                    IsDisplay = isDisplay
                });
            }
            aggregator.SendMessage(viewName + "登录成功！", "Login");
            regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate(viewName);
        }
    }
}
