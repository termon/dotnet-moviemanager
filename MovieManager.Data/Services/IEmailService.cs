
using System.Threading.Tasks;

namespace MovieManager.Data.Services
{
    public interface IEmailService
    {     
        bool Send(string to, string subject, string body, string from=null);         
    }

}