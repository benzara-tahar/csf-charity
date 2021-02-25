using CSF.Charity.Application.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CSF.Charity.Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {

        public string ConvertToBase64String(IFormFile file)
        {
            if (file?.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    return Convert.ToBase64String(fileBytes);

                }
            }
            return null;
        }
    }
}
