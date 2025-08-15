using Face.Common;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.Extensions
{
    public static class DialogExtensions
    {
        public static async Task<IDialogResult> Question(this IDialogHostService dialogHost,string title,string content,string dialogHostName = "Root")
        {
            DialogParameters param= new DialogParameters();
            param.Add("Title",title);
            param.Add("Content", content);
            param.Add("dialogHostName", dialogHostName);
            var dialogResult = await dialogHost.ShowDialog("MsgView", param, dialogHostName);
            return dialogResult;
        }
    }
}
