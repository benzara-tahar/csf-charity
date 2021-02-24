using CSF.Charity.Application.Common.Abstractions;
using CSF.Charity.Domain.Entities;
using System;

namespace CSF.Charity.Application.Repositories
{
    public interface IStateRepository : IRepository<State,Guid>
    {
    }
}
