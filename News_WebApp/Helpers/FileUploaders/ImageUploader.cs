using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace News_WebApp_API.Helpers
{
    public class ImageUploader
    {

        public static async Task<string> UploadImage(IFormFile File, string path)
        {

            if (File.Length > 0 && File != null)
            {
                string ImageName = Guid.NewGuid().ToString() + ".jpg";
                var filepath = Path.Combine(path, ImageName);

                using (var stream = System.IO.File.Create(filepath))
                {
                    await File.CopyToAsync(stream);

                }

                return ImageName;
            }
            else
                return "";
        }


        public static void DeleteImage(string path, string fileName)
        {
            File.Delete(Path.Combine(path, fileName));

        }


    }
}