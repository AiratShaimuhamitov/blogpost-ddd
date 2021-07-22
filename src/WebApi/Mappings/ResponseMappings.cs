using AutoMapper;
using Blogpost.Application.Articles.Queries.GetArticlesWithPagination;
using Blogpost.Application.Comments.Queries.GetComments;
using Blogpost.Application.Common.Models;
using Blogpost.Application.Posts.Queries.GetPostsWithPagination;
using Blogpost.Application.Profiles.Queries.Models;
using Blogpost.WebApi.Models;

namespace Blogpost.WebApi.Mappings
{
    public class ResponseMappings : Profile
    {
        public ResponseMappings()
        {
            CreateMap<PostDto, Post>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(x => new CreatedBy { Id = x.CreatedById, Name = x.CreatedByName }));

            CreateMap<PaginatedList<PostDto>, GetPostsResponse>();

            CreateMap<ArticleDto, Article>();

            CreateMap<PaginatedList<ArticleDto>, GetArticlesResponse>();

            CreateMap<CommentDto, CommentBase>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(x => new CreatedBy { Id = x.CreatedById, Name = x.CreatedByName }));

            CreateMap<CommentDto, SubComment>()
                .IncludeBase<CommentDto, CommentBase>();

            CreateMap<CommentDto, Comment>()
                .ForMember(d => d.SubComments, opt => opt.MapFrom(s => s.SubComments))
                .IncludeBase<CommentDto, CommentBase>();

            CreateMap<MyProfileDto, MyProfile>();

            CreateMap<ProfileDto, UserProfile>();

            CreateMap<ProfileDto, Subscriber>();

            CreateMap<ProfileDto, Subscription>();
        }
    }
}