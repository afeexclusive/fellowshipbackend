
using Fellowship.DTOs;
using Fellowship.Services.FileManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fellowship.Controllers
{
    /// <summary>
    /// Cloud file storage controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly IFileServices fileServices;

        public FileController(IWebHostEnvironment env, IFileServices fileServices)
        {
            this.env = env;
            this.fileServices = fileServices;
        }

        

        [HttpPost("uploadfile")]
        public async Task<IActionResult> Index([FromForm]FileUploadDto model)
        {
            if (model.File.Length < 1)
            {
                return Ok("File could not be found");
            }

            string path = Path.Combine(env.WebRootPath, $"filestore/{model.FolderName}");

            var result = Ok(await fileServices.FirebaseFileUpload(model, path));

            return result;
        }


    }
}
