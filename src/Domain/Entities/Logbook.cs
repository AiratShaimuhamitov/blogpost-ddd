using System;
using Blogpost.Domain.Common;

namespace Blogpost.Domain.Entities
{
    public class Logbook : Entity<long>
    {
        public virtual Profile Profile { get; private init; }

        public DateTime LogDate { get; private init; }

        protected Logbook() { }

        public static Logbook CreateLog(Profile profile, DateTime logDate)
        {
            return new() { Profile = profile, LogDate = logDate };
        }
    }
}