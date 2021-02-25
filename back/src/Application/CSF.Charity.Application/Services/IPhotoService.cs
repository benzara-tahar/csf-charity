using CSF.Charity.Application.TodoLists.Queries.ExportTodos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CSF.Charity.Application.Services
{
    public interface IPhotoService
    {
        string ConvertToBase64String(IFormFile file);
    }
}
