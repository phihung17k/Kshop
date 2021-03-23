using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeTypes;
using KShop.Models;
using Microsoft.AspNetCore.Hosting;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace KShop.Controllers {

    [Route("api/addNewProduct")]
    [ApiController]
    public class UploadController: ControllerBase {

        private readonly IHostingEnvironment _env;

        #region use for Firebase Storage
        //private static string ApiKey = "AIzaSyCuDIlW81eh3kdK5adxdpTz2kLWnlVYouU";
        //private static string Bucket = "kshop-e1db4.appspot.com";
        //private static string AuthEmail = "h@gmail.com";
        //private static string AuthPassword = "123456";
        #endregion

        string fileUploadPath = "";
        readonly Func<string, string, string> GetFilePath = (uploadType, folderPath) => Path.Combine(folderPath, $"{uploadType}");

        public UploadController(IHostingEnvironment env) {
            _env = env;
            fileUploadPath = Path.Combine(_env.WebRootPath, @"images\");
        }

        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig {
            AuthSecret = "1S5oSF1XtjD7CgcI2y4NEa6nDaaglVEryVUJgA9K",
            BasePath = "https://kshop-e1db4-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;



        private void LoadFirebase() {
            client = new FireSharp.FirebaseClient(config);
            if(client != null) {
            }
        }

        protected static string GetBase64StringForImage(string imgPath) {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        private byte[] ObjectToByteArray(Object obj) {
            if(obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        [HttpPost]
        public async Task<ActionResult> UploadAsync([FromForm] FormCreateProduct formData) {
            string output = "";
            LoadFirebase();
            try {

                if(formData.File == null) {
                    return new UnsupportedMediaTypeResult();
                }
                string filename = formData.File.FileName;

                #region algorithm to check file exists. if true, rename file
                if(System.IO.File.Exists(fileUploadPath + filename)) {
                    while(System.IO.File.Exists(fileUploadPath + filename)) {
                        int lastIndex = filename.LastIndexOf(".");
                        string input = filename.Substring(lastIndex - 3, 3);
                        if(Regex.IsMatch(input, "^\\(\\d+\\)$")) {
                            string number = filename.Substring(lastIndex - 2, 1);
                            int num = Int32.Parse(number);
                            num++;
                            filename = filename.Replace("(" + number + ")", "(" + num + ")");
                        } else {
                            filename = filename.Insert(lastIndex, "(1)");
                        }
                    }
                    string oldPath = Path.Combine(fileUploadPath, formData.File.FileName);
                    string newPath = Path.Combine(fileUploadPath, filename);
                    System.IO.File.Move(oldPath, newPath);
                }
                #endregion

                #region save file to path project
                var fileExtension = MimeTypeMap.GetExtension(formData.File.ContentType);
                using var stream = System.IO.File.Create(Path.Combine(fileUploadPath, formData.File.FileName));
                formData.File.CopyToAsync(stream).Wait();
                stream.Close();
                #endregion

                #region upload file on Firebase - Realtime Database
                string base64 = GetBase64StringForImage(GetFilePath(filename, fileUploadPath));
                output = base64;
                var data = new ImageModel {
                    Img = base64
                };


                PushResponse response = await client.PushTaskAsync("Image/", data);
                ImageModel result = response.ResultAs<ImageModel>();
                #endregion

                
                //HttpContext.Items.Add("imageLocation", response.Result.Name);
                //HttpContext.Items.Add("formData", formData);
                HttpContext.Session.Set("formData", ObjectToByteArray(formData));
                HttpContext.Session.Set("imageLocation", Encoding.ASCII.GetBytes(response.Result.Name));

                return Redirect("Product/addProduct");

            } catch(Exception) {
                throw;
                return BadRequest();
            }
            
        }

        //public async Task<IActionResult> Index([FromForm] FileFormData formData) {
        //    var file = formData.File;
        //    FileStream fs;
        //    FileStream ms;
        //    if(file.Length > 0) {
        //        string folderName = "i";
        //        string path = Path.Combine(_env.WebRootPath, $"images/");

        //        if(Directory.Exists(path)) {
        //            using(ms = new FileStream(Path.Combine(path, formData.File.FileName), FileMode.Create)) {
        //                await formData.File.CopyToAsync(ms);
        //            }
        //            ms = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
        //        } else {
        //            Directory.CreateDirectory(path);
        //        }

        //        ms = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
        //        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        //        var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

        //        // you can use CancellationTokenSource to cancel the upload midway
        //        var cancellation = new CancellationTokenSource();

        //        var task = new FirebaseStorage(
        //            Bucket,
        //            new FirebaseStorageOptions {
        //                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
        //                ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
        //            })
        //            .Child("images")
        //            .Child(formData.File.FileName)
        //            .PutAsync(ms, cancellation.Token);

        //        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");


        //        try {
        //            string link = await task;
        //            return Ok();
        //        } catch(Exception ex) {
        //            Console.WriteLine( $"Exception was thrown: {ex}");
        //        }

        //    }
        //    return BadRequest();
        //}
        //public async Task<IActionResult> FormDataUpload([FromForm] FileFormData formData) {

        //    // Check if the request contains multipart/form-data.
        //    if(formData.File == null) {
        //        return new UnsupportedMediaTypeResult();
        //    }
        //    string filename = formData.File.FileName;
        //    //var fileExtension = MimeTypeMap.GetExtension(formData.File.ContentType);
        //    //using var stream = System.IO.File.Create(GetFilePath(filename, fileUploadPath));
        //    //formData.File.CopyToAsync(stream).Wait();

        //    FileStream fileStream=null;
        //    //string path = Path.Combine(_env.WebRootPath, $"images/"+filename);
        //    string path = Path.Combine(_env.WebRootPath, $"images");
        //    //if(Path.GetFileName(path).Equals(filename)) {
        //    //    int lastIndex = path.LastIndexOf(".");
        //    //    path = path.Insert(lastIndex, "(1)");
        //    //}
        //    if(Directory.Exists(path)) {
        //        //if(Path.GetFullPath(path + "/" + filename+"."+formData.File)) {

        //        //}
        //        using(fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create)) {
        //            await formData.File.CopyToAsync(fileStream);
        //        }
        //        fileStream = new FileStream(Path.Combine(path, filename), FileMode.Open);

        //    } else {
        //        Directory.CreateDirectory(path);
        //    }


        //    if(formData.File.Length > 0) {
        //        await Task.Run(() => UploadToFirebase(fileStream, filename));
        //        return Ok();
        //    }
        //    return BadRequest();
        //}


        //private async void UploadToFirebase(FileStream stream, string fileName) {
        //    // FirebaseStorage.Put method accepts any type of stream.
        //    //var stream = new MemoryStream(Encoding.ASCII.GetBytes("Hello world!"));
        //    //var stream = File.Open(@"C:\someFile.png", FileMode.Open);

        //    // of course you can login using other method, not just email+password
        //    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
        //    var a = await auth.SignInWithEmailAndPasswordAsync(authEmail, authPassword);

        //    // you can use CancellationTokenSource to cancel the upload midway
        //    var cancellation = new CancellationTokenSource();

        //    var task = new FirebaseStorage(bucket)
        //        //bucket,
        //        //new FirebaseStorageOptions {
        //        //    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
        //        //    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
        //        //})
        //        .Child("images/")
        //        .Child(fileName)
        //        .PutAsync(stream, cancellation.Token);

        //    task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

        //    // cancel the upload
        //    // cancellation.Cancel();

        //    try {
        //        string link = await task;
        //        // error during upload will be thrown when you await the task
        //        Console.WriteLine("Download link:\n" + link);
        //    } catch(Exception ex) {
        //        Console.WriteLine("Exception was thrown: {0}", ex);
        //    }
        //}

    }
}