using System;

namespace Blogpost.Application.Posts.Queries.GetPostsWithPagination
{
    public class PostDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public bool IsVisible { get; set; }

        public int Likes { get; set; }

        public int Comments { get; set; }

        public Guid CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool HasLikeFromCurrentUser { get; set; }
    }
}