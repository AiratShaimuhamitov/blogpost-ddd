using System;
using System.Linq;
using Blogpost.Domain.Entities;
using Xunit;

namespace Domain.UnitTests.Entities
{
    public class CommentTests
    {
        [Fact]
        public void Add_SubComments_To_Comment_Works_Fine()
        {
            // arrange
            var post = new Post("Test post content");
            var sut = post.AddComment("Test comment content");

            // act
            sut.AddSubComment("Test sub-comment content");

            // assert
            Assert.Equal(1, sut.SubComments.Count);
        }

        [Fact]
        public void Exception_IsThrown_On_Adding_SubComment_To_SubComment()
        {
            // arrange
            var post = new Post("Test post content");
            var comment = post.AddComment("Test comment content");
            var sut = comment.AddSubComment("Test first sub-comment content");

            // act && assert
            Assert.Throws<InvalidOperationException>(() =>
                sut.AddSubComment("Test sub-sub-comment content"));
        }

        [Fact]
        public void Remove_SubComment_From_Comment_Works_Fine()
        {
            // arrange
            var post = new Post("Test post content");
            var sut = post.AddComment("Test comment content");
            var subComment = sut.AddSubComment("Test sub-comment content");

            // act
            sut.RemoveSubComment(subComment);

            // assert
            Assert.Equal(0, sut.SubComments.Count);
        }
    }
}