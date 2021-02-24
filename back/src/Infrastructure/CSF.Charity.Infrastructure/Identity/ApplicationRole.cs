using AspNetCore.Identity.MongoDbCore.Models;
using CSF.Charity.Domain.Core.Models;

namespace CSF.Charity.Infrastructure.Identity
{


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
