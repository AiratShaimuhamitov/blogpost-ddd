using System;
using System.Collections.Generic;
using System.Linq;

namespace Blogpost.Domain.Entities
{
    public class Comment : FeedItem<Guid>
    {
        public string Content { get; private init; }
        public virtual Comment Parent { get; private set; }

        private readonly IList<Comment> _subComments = new List<Comment>();

        public virtual IReadOnlyCollection<Comment> SubComments => _subComments.ToList();

        protected Comment() { }

        internal Comment(string content)
        {
            Content = content;
        }

        public Comment AddSubComment(string content)
        {
            if (Parent is not null)
            {
                throw new InvalidOperationException("Sub comment can't have own sub comments");
            }

            var subComment = new Comment { Content = content };
            _subComments.Add(subComment);
            subComment.Parent = this;

            return subComment;
        }

        public void RemoveSubComment(Comment comment)
        {
            _subComments.Remove(comment);
        }
    }
}