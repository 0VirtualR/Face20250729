using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Shared
{
    public class FaceDto:BaseDto
    {

		//public bool isSelected
		//{
		//	get => _isSelected;
		//	set { _isSelected = value; OnPropertyChanged(); }
		//}

		//private bool _isSelected;
		//public bool isSelected
		//{
		//	get => _isSelected;
		//	set => SetProperty(ref _isSelected, value);
		//}


		private bool _isSelected;
		public bool isSelected
		{
			get => _isSelected;
			set { _isSelected = value; OnPropertyChanged(); }
		}

		private string _name;
		public string name
		{
			get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

		private string _sex;
		public string sex
		{
			get => _sex;
            set { _sex  = value; OnPropertyChanged(); }
        }

		private string _workId;
		public string workId
		{
			get => _workId;
            set { _workId = value; OnPropertyChanged(); }
        }

		private string _workName;
		public string workName
		{
			get => _workName;
            set { _workName = value; OnPropertyChanged(); }
        }

		private DateTime _createTime;
		public DateTime createTime
		{
			get => _createTime;
            set { _createTime = value; OnPropertyChanged(); }
        }

	}
}
