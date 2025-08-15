using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Common
{
    public interface IDialogHostAware
    {
        public string DialogHostName { get; set; }
        void OnDialogOpend(IDialogParameters parameters);
        DelegateCommand SaveCommand { get; set;}
        DelegateCommand CancelCommand { get; set;}
    }
}
