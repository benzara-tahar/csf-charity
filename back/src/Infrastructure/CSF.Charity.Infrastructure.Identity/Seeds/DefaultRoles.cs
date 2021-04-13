using CSF.Charity.Domain.Enums;
using CSF.Charity.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace CSF.Charity.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(BuiltInRoles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(BuiltInRoles.AssociationAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(BuiltInRoles.User.ToString()));
        }
    }
}
