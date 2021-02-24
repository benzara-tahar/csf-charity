using AspNetCore.Identity.MongoDbCore.Models;
using CSF.Charity.Domain.Core.Models;

namespace CSF.Charity.Infrastructure.Identity
{

    public class ApplicationUser :  MongoIdentityUser<string>, IEntity<string>
    {
    }
}
