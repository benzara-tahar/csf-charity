using AspNetCore.Identity.MongoDbCore.Models;
using CSF.Charity.Domain.Common;
using CSF.Charity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CSF.Charity.Infrastructure.Identity
{
    [CollectionName("roles")]

    public class ApplicationRole : MongoIdentityRole<string>, IEntity<string>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
