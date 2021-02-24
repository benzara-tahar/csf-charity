using System;
using System.Threading.Tasks;

namespace CSF.Charity.Application.Common.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();
    }
}
