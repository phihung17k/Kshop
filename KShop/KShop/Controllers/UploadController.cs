using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MimeTypes;
using KShop.Models;
using Microsoft.AspNetCore.Hosting;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


namespace KShop.Controllers {

    [Route("api/uploadImageProduct")]
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


        [HttpPost]
        public async Task<ActionResult> UploadAsync([FromForm] FormProduct formData) {
            string output = "";
            LoadFirebase();
            try {
                bool checkFile = true;
                if("Create".Equals(formData.Button)) {
                    if(formData.File == null) {
                        return new UnsupportedMediaTypeResult();
                    }
                } else if ("Update".Equals(formData.Button)){
                    if(formData.File == null) {
                        checkFile = false;
                    }
                }

                PushResponse response = null;
                if(checkFile) {
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

                    response = await client.PushTaskAsync("Image/", data);
                    ImageModel result = response.ResultAs<ImageModel>();
                    #endregion
                }
               
                TempForm tempForm = new TempForm();
                if("Create".Equals(formData.Button)) {
                    tempForm.ImageLocation = response.Result.Name;
                } else if("Update".Equals(formData.Button)) {
                    tempForm.ProductId = formData.ProductId;
                    if(formData.File != null) {
                        tempForm.ImageLocation = response.Result.Name;
                    }
                }
                tempForm.ProductName = formData.ProductName;
                tempForm.Quantity = formData.Quantity;
                tempForm.Price = formData.Price;
                tempForm.CategoryId = formData.CategoryId;
                

                string serialize = System.Text.Json.JsonSerializer.Serialize<TempForm>(tempForm);
                HttpContext.Session.SetString("tempForm", serialize);

                return Redirect("Product/addProduct");
            } catch(Exception) {
                throw;
            }
            return BadRequest();
        }


    }
}