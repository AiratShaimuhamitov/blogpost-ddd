using System;
using System.Collections.Generic;

namespace Blogpost.Application.Comments.Queries.GetComments
{
    public class CommentDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public List<CommentDto> SubComments { get; } = new();

        public Guid CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool HasLikeFromCurrentUser { get; set; }
    }
}