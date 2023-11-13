using Forces.Application.Requests.Mail;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}