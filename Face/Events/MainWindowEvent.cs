using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Face.Events
{
    public class DisplayImg
    {
        public bool IsDisplay {  get; set; }
    }
  public  class MainWindowEvent:PubSubEvent<DisplayImg>
    {
    }
}
