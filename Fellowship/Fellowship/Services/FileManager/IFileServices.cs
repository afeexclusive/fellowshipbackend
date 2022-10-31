using Fellowship.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Services.FileManager
{
    public interface IFileServices
    {
        Task<ResponseModel> FirebaseFileUpload(FileUploadDto model, string path);
    }
}
