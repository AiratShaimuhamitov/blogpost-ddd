using System;
using System.Collections.Generic;

namespace Blogpost.Domain.Entities
{
    /// <summary>
    /// Post - aggregate root
    /// </summary>
    public class Post : FeedItem<Guid>
    {
        public string Content { get; private set; }

        public bool IsVisible { get; private set; }

        private readonly List<Comment> _comments = new();
        public virtual IReadOnlyList<Comment> Comments => _comments;

        protected Post()
        {
        }

        public Post(string content, bool isVisible = true)
        {
            Content = content;
            IsVisible = isVisible;
        }

        public Comment AddComment(string content)
        {
            var comment = new Comment(content);
            _comments.Add(comment);

            return comment;
        }

        public void MakeVisible()
        {
            if (IsVisible) throw new InvalidOperationException("The post is already visible");

            IsVisible = true;
        }

        public void MakeInvisible()
        {
            if (!IsVisible) throw new InvalidOperationException("The post is already invisible");

            IsVisible = false;
        }
    }
}