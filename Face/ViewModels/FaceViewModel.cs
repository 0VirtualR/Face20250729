using Face.Extensions;
using Face.Interface;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Face.ViewModels
{
    public class FaceViewModel:NavigateViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IFaceService faceService;
        private ObservableCollection<FaceDto> faceDtos;

        public ObservableCollection<FaceDto> FaceDtos
        {
            get { return faceDtos; }
            set { faceDtos = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> NavigateCommand { get; private set; }
      public  FaceViewModel (IRegionManager regionManager,IFaceService faceService)
        {
            this.regionManager = regionManager;
            this.faceService = faceService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            FaceDtos=new ObservableCollection<FaceDto> ();
            
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
            var faceinfos =await faceService.GetAll(new QueryParameter()
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
