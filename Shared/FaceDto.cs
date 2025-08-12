using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class FaceDto:BaseDto
    {
		private string name;

		public string Name
		{
			get { return name; }
			set { name = value;OnPropertyChanged(); }
		}
		private string sex;

		public string Sex
		{
			get { return sex; }
			set { sex = value; OnPropertyChanged(); }
		}
		private string workId;

		public string WorkId
		{
			get { return workId; }
			set { workId = value; OnPropertyChanged(); }
		}
		private string workName;

		public string WorkName
		{
			get { return workName; }
			set { workName = value; OnPropertyChanged(); }
		}
		private DateTime createDate;

		public DateTime CreateDate
		{
			get { return createDate; }
			set { createDate = value; OnPropertyChanged();}
		}

	}
}
