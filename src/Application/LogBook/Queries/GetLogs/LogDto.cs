using System;

namespace Blogpost.Application.LogBook.Queries.GetLogs
{
    public record LogDto
    {
        public DateTime LogDate { get; set; }
    }
}