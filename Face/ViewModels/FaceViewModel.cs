using Face.Common;
using Face.Extensions;
using Face.Interface;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Face.ViewModels
{
    public class FaceViewModel:NavigateViewModel
    {

        #region 字段属性
       public Dictionary<string, object> ReturnMainViewParam { get; } = new Dictionary<string, object>
        {
            {"IsPublish",true },
            {"IsDisplay",true }
        };

        private ObservableCollection<FaceDto> faceDtos;
        public ObservableCollection<FaceDto> FaceDtos
        {
            get => faceDtos;
            set => SetProperty(ref faceDtos, value);
        }
      
        private FaceDto currentDto;
        public FaceDto CurrentDto
        {
            get => currentDto;
            set =>SetProperty(ref currentDto, value);
        }
        #endregion

        #region 初始化
        private readonly IDialogService dialogService;
        private readonly IFaceService faceService;
        private readonly IDialogHostService dialogHost;
        public DelegateCommand<FaceDto> EditCommand { get; private set; }
        public DelegateCommand<string> SaveDtoCommand { get; private set; }
      public  FaceViewModel (IRegionManager regionManager,IEventAggregator aggregator, IDialogService dialogService, IFaceService faceService,IDialogHostService dialogHost):base(regionManager,aggregator)
        {
            this.dialogService = dialogService;
            this.faceService = faceService;
            this.dialogHost = dialogHost;
            FaceDtos=new ObservableCollection<FaceDto> ();
            SaveDtoCommand = new DelegateCommand<string>(SaveDto);
            EditCommand = new DelegateCommand<FaceDto>(Edit);

            ToggleSelectAllCommand = new DelegateCommand(ToggleSelectAll);
            
        }
        #endregion


        #region 人员信息的datagrid所需的函数
        //全选和取消全选的表头操作实现函数
        // 计算属性：是否所有行都被选中

        public DelegateCommand ToggleSelectAllCommand { get; private set; }
        public void ToggleSelectAll()
        {
            if (FaceDtos == null || FaceDtos.Count == 0) return;

            // 检查是否已经全选
            bool allSelected = FaceDtos.All(item => item.isSelected);

            // 切换选择状态
            bool newState = !allSelected;
            foreach (var item in FaceDtos)
            {
                item.isSelected = newState;
            }
        }
        #endregion

        #region 函数实现

        private void Edit(FaceDto dto)
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
                        old.sex = resdb.Result.sex;
                        old.workName = resdb.Result.workName;
                        old.workId = resdb.Result.workId;
                        old.name = resdb.Result.name;
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
        public async override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var faceinfos = await faceService.GetAllAsync(new QueryParameter()
            {
                PageSize = 11,
                PageIndex = 1
            });
            if (faceinfos.Status)
            {
                FaceDtos.Clear();
                foreach (var item in faceinfos.Result.Items)
                {
                    FaceDtos.Add(item);
                }
            }
        }
        #endregion
    }
}
