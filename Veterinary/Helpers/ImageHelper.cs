using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            string guid = Guid.NewGuid().ToString();
            string file = $"{guid}.jpg";
            Directory.CreateDirectory($"wwwroot\\images\\{folder}");
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                            $"wwwroot\\images\\{folder}",
                            file);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"~/images/{folder}/{file}";

        }

        public bool ValidFileTypes(IFormFile file)
        {
            string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg" };
            string ext = Path.GetExtension(file.FileName).ToLower();
            bool isValidFile = false;
            for (int i = 0; i < validFileTypes.Length; i++)
            {
                if (ext == "." + validFileTypes[i])
                {
                    isValidFile = true;
                    break;
                }
            }


            return isValidFile;
        }
    }
}
