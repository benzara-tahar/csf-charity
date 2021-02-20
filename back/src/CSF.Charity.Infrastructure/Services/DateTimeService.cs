using CSF.Charity.Application.Common.Interfaces;
using System;

namespace CSF.Charity.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
