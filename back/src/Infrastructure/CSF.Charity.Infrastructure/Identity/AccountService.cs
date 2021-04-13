using CSF.Charity.Application.Common.Models;
using CSF.Charity.Application.Services;
using CSF.Charity.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSF.Charity.Infrastructure.Identity
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id.ToString() == userId);

            return user.UserName;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id.ToString());
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id.ToString() == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id.ToString() == userId);

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id.ToString() == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<IList<ApplicationUser>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<ApplicationUser> RegisterUser(ApplicationUser entity, string password, string role)
        {
            var user = await _userManager.FindByNameAsync(entity.UserName);

            if (user != null)
            {
                throw new Exception($"User Already Exists");
            }

            var result = await _userManager.CreateAsync(entity, password);

            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                throw new Exception($"User Registration Failed");
            }
            user = await _userManager.FindByNameAsync(entity.UserName);
            await EnsureRoleExistsAsync(role);
            result = await _userManager.AddToRoleAsync(user, role);

            return user;
        }

        private async Task EnsureRoleExistsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                var result  = await _roleManager.CreateAsync(new ApplicationRole(roleName));
            };
        }

        public async Task<ApplicationUser> UpdateUser(ApplicationUser entity)
        {
            var result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                throw new Exception($"User Update Failed");
            }

            var user = await _userManager.FindByNameAsync(entity.UserName);
            return user;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception($"User Does Not Exist");
            }

            await _userManager.DeleteAsync(user);
            return true;
        }

        public IList<ApplicationRole> GetRoles()
        {
            var roles = this._roleManager.Roles.ToList();
            return roles;
        }

        public async Task<IList<ApplicationUser>> GetUsersByAssociation(string associationId)
        {
            var users = await _userManager
                .Users
                .Where(e => e.AssociationId == associationId)
                .ToListAsync();
            return users.ToList();
        }
    }
}
