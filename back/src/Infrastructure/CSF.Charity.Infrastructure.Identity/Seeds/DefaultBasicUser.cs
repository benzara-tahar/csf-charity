using CSF.Charity.Domain.Enums;
using CSF.Charity.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CSF.Charity.Infrastructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "basicuser",
                Email = "basicuser@gmail.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@ssw0rd");
                    await userManager.AddToRoleAsync(defaultUser, BuiltInRoles.User.ToString());
                }

            }
        }
    }
}
