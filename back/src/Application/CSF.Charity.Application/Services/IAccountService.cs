using CSF.Charity.Application.Common.Models;
using CSF.Charity.Domain.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Services
{
    public interface IAccountService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<IList<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<ApplicationUser> RegisterUser(ApplicationUser entity, string password, string role);
        Task<ApplicationUser> UpdateUser(ApplicationUser entity);
        Task<bool> DeleteUser(string userId);
        IList<ApplicationRole> GetRoles();
        Task<IList<ApplicationUser>> GetUsersByAssociation(string associationId);
    }
}
