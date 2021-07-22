using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Blogpost.Application.Comments.Commands.AddComment;
using Blogpost.Application.Comments.Commands.AddSubComment;
using Blogpost.Application.Comments.Commands.Delete;
using Blogpost.Application.Comments.Commands.Like;
using Blogpost.Application.Comments.Commands.Unlike;
using Blogpost.Application.Comments.Queries.GetComments;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.Posts.Commands.Create;
using Blogpost.Application.Posts.Commands.Delete;
using Blogpost.Application.Posts.Commands.Like;
using Blogpost.Application.Posts.Commands.Unlike;
using Blogpost.Application.Posts.Queries.GetPostById;
using Blogpost.Application.Posts.Queries.GetPostsWithPagination;
using Blogpost.WebApi.Attributes;
using Blogpost.WebApi.Models;

namespace Blogpost.WebApi.Controllers
{
    [Authorize]
    public class PostsController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public PostsController(ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// Создание нового комментария
        /// </summary>
        [HttpPost]
        [Route("{postId:guid}/comments")]
        [ValidateModelState]
        [SwaggerOperation("CreateNewComment")]
        [SwaggerResponse(statusCode: 200, type: typeof(Guid?), description: "Success")]
        [SwaggerResponse(statusCode: 422, type: typeof(Error), description: "Unprocessable entity")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> CreateNewComment([FromBody] CreateCommentRequest body, [FromRoute][Required] Guid? postId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new AddCommentRequest
            {
                PostId = postId!.Value,
                Content = body.Content
            }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Создание нового поста
        /// </summary>
        [HttpPost]
        [ValidateModelState]
        [SwaggerOperation("CreateNewPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(Guid?), description: "Success")]
        [SwaggerResponse(statusCode: 422, type: typeof(Error), description: "Unprocessable entity")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> CreateNewPost([FromBody] CreatePostRequest body, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new CreatePostCommand
            {
                Content = body.Content,
                IsVisible = body.IsVisible ?? true,
                CreatorId = _currentUserService.UserId!.Value
            }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Создание нового дочерного комментария
        /// </summary>
        [HttpPost]
        [Route("/api/comments/{commentId:guid}/subComments")]
        [ValidateModelState]
        [SwaggerOperation("CreateNewSubComment")]
        [SwaggerResponse(statusCode: 200, type: typeof(Guid?), description: "Success")]
        [SwaggerResponse(statusCode: 422, type: typeof(Error), description: "Unprocessable entity")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> CreateNewSubComment([FromBody] CreateCommentRequest body, [FromRoute][Required] Guid? commentId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new AddSubCommentRequest
            {
                CommentId = commentId!.Value,
                Content = body.Content
            }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Удаление комментария по идентификатору
        /// </summary>
        [HttpDelete]
        [Route("/api/comments/{commentId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteCommentById")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 405, type: typeof(Error), description: "Method Not Allowed")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> DeleteCommentById([FromRoute][Required] Guid? commentId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteCommentCommand
            {
                CommentId = commentId!.Value
            }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаление лайка с комментария
        /// </summary>
        [HttpDelete]
        [Route("/api/comments/{commentId:guid}/likes/{profileId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteLikeFromComment")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> DeleteLikeFromComment([FromRoute][Required] Guid? commentId, [FromRoute][Required] Guid? profileId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new UnlikeCommentCommand
            {
                CommentId = commentId!.Value,
                ProfileId = _currentUserService.UserId!.Value
            }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаление лайка с поста
        /// </summary>
        [HttpDelete]
        [Route("{postId:guid}/likes/{profileId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteLikeFromPost")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 405, type: typeof(Error), description: "Method Not Allowed")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> DeleteLikeFromPost([FromRoute][Required] Guid? postId, [FromRoute][Required] Guid? profileId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new UnlikePostCommand
            {
                PostId = postId!.Value,
                ProfileId = _currentUserService.UserId!.Value
            }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаление поста по идентификатору
        /// </summary>
        [HttpDelete]
        [Route("{postId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("DeletePostById")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 405, type: typeof(Error), description: "Method Not Allowed")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> DeletePostById([FromRoute][Required] Guid? postId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeletePostCommand
            {
                PostId = postId!.Value
            }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Получение всех постов
        /// </summary>
        [HttpGet]
        [ValidateModelState]
        [SwaggerOperation("GetAllPosts")]
        [SwaggerResponse(statusCode: 200, type: typeof(GetPostsResponse), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GetAllPosts([FromQuery] int size = 10, [FromQuery] int page = 1, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(
                new GetPostsWithPaginationQuery { PageSize = size, PageNumber = page }, cancellationToken);

            return Ok(_mapper.Map<GetPostsResponse>(result));
        }

        /// <summary>
        /// Получение поста по идентификатору
        /// </summary>
        [HttpGet]
        [Route("{postId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("GetPostById")]
        [SwaggerResponse(statusCode: 200, type: typeof(Post), description: "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GetPostById([FromRoute][Required] Guid? postId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(
                new GetPostByIdQuery { PostId = postId!.Value }, cancellationToken);

            return Ok(_mapper.Map<Post>(result));
        }

        /// <summary>
        /// Получение комментариев поста
        /// </summary>
        [HttpGet]
        [Route("{postId:guid}/comments")]
        [ValidateModelState]
        [SwaggerOperation("GetPostComments")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Comment>), description: "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GetPostComments([FromRoute][Required] Guid? postId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(
                new GetCommentsQuery { PostId = postId!.Value }, cancellationToken);

            return Ok(_mapper.Map<List<Comment>>(result));
        }

        /// <summary>
        /// Добавление лайка к комментарию
        /// </summary>
        [HttpPut]
        [Route("/api/comments/{commentId:guid}/likes/{profileId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("PutLikeToComment")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 405, type: typeof(Error), description: "Method Not Allowed")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> PutLikeToComment([FromRoute][Required] Guid? commentId, [FromRoute][Required] Guid? profileId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new LikeCommentCommand
            {
                CommentId = commentId!.Value,
                ProfileId = _currentUserService.UserId!.Value
            }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Добавление лайка к посту
        /// </summary>
        [HttpPut]
        [Route("{postId:guid}/likes/{profileId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("PutLikeToPost")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "Not NotFound")]
        [SwaggerResponse(statusCode: 405, type: typeof(Error), description: "Method Not Allowed")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> PutLikeToPost([FromRoute][Required] Guid? postId, [FromRoute][Required] Guid? profileId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new LikePostCommand
            {
                PostId = postId!.Value,
                ProfileId = _currentUserService.UserId!.Value
            }, cancellationToken);

            return NoContent();
        }
    }
}