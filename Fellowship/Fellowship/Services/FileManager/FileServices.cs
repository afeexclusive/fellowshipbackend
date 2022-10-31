using Fellowship.DTOs;
using Fellowship.Helper;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fellowship.Services.FileManager
{
    public class FileServices : IFileServices
    {
        private readonly FirebaseFileStore firebaseStore;

        public FileServices(IOptions<FirebaseFileStore> firebaseStore)
        {
            this.firebaseStore = firebaseStore.Value;
        }

        public async Task<ResponseModel> FirebaseFileUpload(FileUploadDto model, string path)
        {
            var file = model.File;
            FileStream fs;
            FileStream ms;
            if (file.Length > 0)
            {
                
                try
                {
                    Directory.CreateDirectory(path);

                    string savepath = Path.Combine(path, file.FileName);

                    using (fs = new FileStream(savepath, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }

                    

                        ms = new FileStream(savepath, FileMode.Open);
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(firebaseStore.ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(firebaseStore.AuthEmail, firebaseStore.AuthPassword);

                    // you can use CancellationTokenSource to cancel the upload midway
                    var cancellation = new CancellationTokenSource();

                    var task = await new FirebaseStorage(
                        firebaseStore.Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                         })
                        .Child(model.FolderName)
                        .Child(file.FileName)
                        .PutAsync(ms, cancellation.Token);
                    return new ResponseModel { Response = "Success", Status = true, ReturnObj = new { FileUrl = task } };
                }
                catch (Exception ex)
                {
                    return new ResponseModel { Response =ex.Message, Status = false };

                }


            }
            return new ResponseModel { Response = "Failed", Status = false };
        }


    }
}
