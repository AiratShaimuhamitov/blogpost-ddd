using System;
using Blogpost.Application.Common.Interfaces;

namespace Blogpost.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}