using Face.Interface;
using Face.Models;
using MyToDo.Shared.Contact;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Face.Service
{
    public class FaceService :BaseService<FaceDto>, IFaceService
    {
        private readonly IApiService apiService;

        public FaceService( IApiService apiService) : base("Face", apiService)
        {
        }
    }
}
