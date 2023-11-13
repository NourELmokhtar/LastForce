using Forces.Application.Requests;
using Forces.Application.Requests.Requests;

namespace Forces.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
        string UploadAsync(AttachmentRequest request);

    }
}