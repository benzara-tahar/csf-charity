using AspNetCore.Identity.Mongo.Model;

namespace CSF.Charity.Domain.Identity
{
    public class ApplicationRole : MongoRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
