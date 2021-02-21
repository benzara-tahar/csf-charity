using AspNetCore.Identity.MongoDbCore.Models;
using CSF.Charity.Domain.Common;
using CSF.Charity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CSF.Charity.Infrastructure.Identity
{
    [CollectionName("users")]

    public class ApplicationUser :  MongoIdentityUser<string>, IEntity<string>
    {
    }
}
