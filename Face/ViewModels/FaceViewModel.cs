using Face.Common;
using Face.Extensions;
using Face.Interface;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Face.ViewModels
{
    public class FaceViewModel:NavigateViewModel
    {
        private readonly IDialogService dialogService;
        private readonly IRegionManager regionManager;
        private readonly IFaceService faceService;
        private readonly IDialogHostService dialogHost;
        private ObservableCollection<FaceDto> faceDtos;

        public ObservableCollection<FaceDto> FaceDtos
        {
            get { return faceDtos; }
            set { faceDtos = value; RaisePropertyChanged(); }
        }
        private FaceDto currentDto;

        public FaceDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value;RaisePropertyChanged(); }
        }
        public DelegateCommand<FaceDto> EditCommand { get; private set; }
        public DelegateCommand<string> SaveDtoCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
      public  FaceViewModel (IDialogService dialogService, IRegionManager regionManager,IFaceService faceService,IDialogHostService dialogHost)
        {
            this.dialogService = dialogService;
            this.regionManager = regionManager;
            this.faceService = faceService;
            this.dialogHost = dialogHost;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            FaceDtos=new ObservableCollection<FaceDto> ();
            SaveDtoCommand = new DelegateCommand<string>(SaveDto);
            EditCommand = new DelegateCommand<FaceDto>(Edit);
            
        }

        private  void Edit(FaceDto dto)
        {
            var param = new  DialogParameters();
            param.Add("Value", dto);
            dialogService.ShowDialog("AddFaceView", param, async res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    var resface = res.Parameters.GetValue<FaceDto>("Value");
                    var resdb = await faceService.UpdateAsync(resface);
                    if (resdb.Status)
                    {
                        //更新成功
                        var old = FaceDtos.FirstOrDefault(x=>x.Id.Equals(resface.Id));
                        old.Sex = resdb.Result.Sex;
                        old.WorkName = resdb.Result.WorkName;
                        old.WorkId = resdb.Result.WorkId;
                        old.Name = resdb.Result.Name;
                       await dialogHost.Question("温馨提示", "更新成功");
                    }
                }
            });
        }

        private void SaveDto(string obj)
        {
            switch (obj)
            {
                case "新增":Add();break;
                case "更新":Update();break;
            }
        }
    
        private void Update()
        {
            throw new NotImplementedException();
        }

        private void Add()
        {
            var param = new DialogParameters();
           if (CurrentDto != null)
            {
                param.Add("Value", CurrentDto);
            }
            dialogService.ShowDialog("AddFaceView", param, async res =>
            {
                if (res.Result == ButtonResult.OK)
                {
                    if (res.Parameters.ContainsKey("Value"))
                    {
                        var face = res.Parameters.GetValue<FaceDto>("Value");
                      var resadd = await faceService.AddAsync(face);
                        if (resadd.Status)
                        {
                            FaceDtos.Add(resadd.Result);
                            //弹窗 添加信息成功
                          await  dialogHost.Question("温馨提示", "新增成功");
                        }
                    }
                }
            });
        }

        private void Navigate(string obj)
        {
            regionManager.Regions[PrismManager.MainWindowRegionName].RequestNavigate(obj);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            GetFaceDtoData();
        }

        async private void GetFaceDtoData()
        {
            var faceinfos =await faceService.GetAllAsync(new QueryParameter()
            {
                PageSize= 11,
                PageIndex=1
            });
            if (faceinfos.Status)
            {
                FaceDtos.Clear();
                foreach(var item in faceinfos.Result.Items)
                {
                    FaceDtos.Add(item);
                }
            }
        }
    }
}
