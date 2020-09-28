using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Veterinary.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);

        bool ValidFileTypes(IFormFile file);
    }
}
