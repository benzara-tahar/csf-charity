using CSF.Charity.Application.Services;
using System;

namespace CSF.Charity.Infrastructure.Services
{
    public class MachineDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
