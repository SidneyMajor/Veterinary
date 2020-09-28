using System.Threading.Tasks;

namespace Veterinary.Helpers
{
    public interface IMailHelper
    {
        Task SendMail(string to, string subject, string body);
    }
}
