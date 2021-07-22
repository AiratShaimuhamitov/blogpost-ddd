using System;
using Blogpost.Domain.Common;

namespace Blogpost.Domain.Entities
{
    public class Article : AggregateRoot<int>
    {
        public string Title { get; private set; }

        public string Content { get; private set; }

        public DateTime Created { get; private set; }

        protected Article() { }
    }
}