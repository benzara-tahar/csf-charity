using CSF.Charity.Application.Common.Abstractions;
using MediatR;
using System.Threading.Tasks;

namespace CSF.Charity.Infrastructure.Persistance.Mongo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;
        private readonly IMediator _mediator;

        public UnitOfWork(IMongoContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<bool> CommitAsync()
        {
            var changeAmount = await _context.SaveChanges();
          //  var domainEvents = _context.GetDomainEvents();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
