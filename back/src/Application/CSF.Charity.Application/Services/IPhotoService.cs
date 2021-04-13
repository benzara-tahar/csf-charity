using Microsoft.AspNetCore.Http;

namespace CSF.Charity.Application.Services
{
    public interface IPhotoService
    {
        string ConvertToBase64String(IFormFile file);
    }
}
