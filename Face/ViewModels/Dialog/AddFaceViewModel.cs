using Face.Interface;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.ViewModels.Dialog
{
    class AddFaceViewModel : BindableBase, IDialogAware
    {
        private FaceDto currentDto;
        private readonly IFaceService faceService;

        public FaceDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public string Title => "添加人脸信息";

        public event Action<IDialogResult> RequestClose;
       public AddFaceViewModel(IFaceService faceService)
        {
    
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            this.faceService = faceService;
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private  void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Name) || string.IsNullOrWhiteSpace(CurrentDto.WorkName) || string.IsNullOrWhiteSpace(CurrentDto.WorkId)) return;
            var param = new DialogParameters();
            param.Add("Value", CurrentDto);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK,param));
        }

        public bool CanCloseDialog()
        {
            //   CanCloseDialog也就是说关闭弹窗之间会进入到这里来执行这个函数获取返回值，判断是否可以关闭，
            // RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param)); 是在CanCloseDialog返回为true的时候才执行
          //  OnDialogClosed对话框关闭后做什么操作



            return true;
        }

        public void OnDialogClosed()
        {
             
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            CurrentDto = new FaceDto();
            if (parameters.ContainsKey("Value"))
            {

                var res = parameters.GetValue<FaceDto>("Value");
                CurrentDto.WorkId = res.WorkId;
                CurrentDto.WorkName = res.WorkName;
                CurrentDto.Name = res.Name;
                CurrentDto.Sex = res.Sex;
                CurrentDto.Id = res.Id;

            }


            
        }
    }
}
