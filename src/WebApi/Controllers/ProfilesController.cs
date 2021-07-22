using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blogpost.Application.Common.Interfaces;
using Blogpost.Application.LogBook.Queries.GetLogs;
using Blogpost.Application.Posts.Queries.GetMyPostsWithPagination;
using Blogpost.Application.Posts.Queries.GetSubscriptionsPostsWithPagination;
using Blogpost.Application.Profiles.Commands.Create;
using Blogpost.Application.Profiles.Commands.Delete;
using Blogpost.Application.Profiles.Commands.Subscribe;
using Blogpost.Application.Profiles.Commands.Unsubscribe;
using Blogpost.Application.Profiles.Queries.GetMyProfile;
using Blogpost.Application.Profiles.Queries.GetProfileById;
using Blogpost.Application.Profiles.Queries.GetSubscribers;
using Blogpost.Application.Profiles.Queries.GetSubscriptions;
using Blogpost.WebApi.Attributes;
using Blogpost.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Blogpost.WebApi.Controllers
{
    [Authorize]
    public class ProfilesController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public ProfilesController(ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        [HttpGet]
        [Route("my")]
        [ValidateModelState]
        [SwaggerOperation("GetMyProfile")]
        [SwaggerResponse(statusCode: 200, type: typeof(MyProfile), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken = default)
        {
            var profile = await Mediator.Send(new GetMyProfileQuery(), cancellationToken);

            return Ok(_mapper.Map<MyProfile>(profile));
        }

        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        [HttpGet]
        [Route("{profileId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("GetProfileById")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserProfile), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GetProfileById([FromRoute][Required] Guid? profileId, CancellationToken cancellationToken = default)
        {
            var profile = await Mediator.Send(new GetProfileByIdQuery { ProfileId = profileId!.Value }, cancellationToken);

            return Ok(_mapper.Map<UserProfile>(profile));
        }

        /// <summary>
        /// Удаление текущего пользователя
        /// </summary>
        [HttpDelete]
        [Route("my")]
        [ValidateModelState]
        [SwaggerOperation("DeleteMyProfile")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> DeleteMyProfile(CancellationToken cancellationToken = default)
        {
            var profileId = _currentUserService.UserId ?? throw new InvalidOperationException("Current userId is null");
            await Mediator.Send(new DeleteProfileCommand { ProfileId = profileId }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Создание профиля
        /// </summary>
        /// <param name="body"></param>
        [HttpPost]
        [ValidateModelState]
        [AllowAnonymous]
        [SwaggerOperation("register")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest body, CancellationToken cancellationToken = default)
        {
            var profileId = await Mediator.Send(new CreateProfileCommand
            {
                Name = body.Name,
                Email = body.Email,
                Password = body.Password
            }, cancellationToken);

            return Ok(profileId);
        }

        /// <summary>
        /// Подписка на другого пользователя
        /// </summary>
        /// <param name="body"></param>
        [HttpPut]
        [Route("my/subscriptions")]
        [ValidateModelState]
        [SwaggerOperation("SubscribeToProfile")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 409, type: typeof(Error), description: "Business logic error")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> SubscribeToProfile([FromBody] SubscribeRequest body, CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId ?? throw new InvalidOperationException("Current userId is null");

            await Mediator.Send(new SubscribeCommand { SubscriberId = userId, ToProfileId = body.ToProfileId }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удаление подписки
        /// </summary>
        /// <param name="profileId">Идентификатор пользователя</param>
        [HttpDelete]
        [Route("my/subscriptions/{profileId:guid}")]
        [ValidateModelState]
        [SwaggerOperation("UnsubscribeFromProfile")]
        [SwaggerResponse(statusCode: 204, "Success")]
        [SwaggerResponse(statusCode: 409, type: typeof(Error), description: "Business logic error")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> UnsubscribeFromProfile([FromRoute][Required] Guid? profileId, CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId ?? throw new InvalidOperationException("Current userId is null");

            await Mediator.Send(new UnsubscribeCommand { SubscriberId = userId, FromProfileId = profileId!.Value }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Получение моих подписчиков
        /// </summary>
        [HttpGet]
        [Route("my/subscribers")]
        [ValidateModelState]
        [SwaggerOperation("GetMySubscribers")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Subscriber>), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<ActionResult> GetMySubscribers(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId ?? throw new InvalidOperationException("Current userId is null");

            var subscribers = await Mediator.Send(new GetSubscribersQuery { ProfileId = userId }, cancellationToken);
            return Ok(_mapper.Map<List<Subscriber>>(subscribers));
        }

        /// <summary>
        /// Получение моих подписок
        /// </summary>
        [HttpGet]
        [Route("my/subscriptions")]
        [ValidateModelState]
        [SwaggerOperation("GetMySubscriptions")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Subscription>), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<ActionResult> GetMySubscriptions(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId ?? throw new InvalidOperationException("Current userId is null");

            var subscriptions = await Mediator.Send(new GetSubscriptionsQuery { ProfileId = userId }, cancellationToken);
            return Ok(_mapper.Map<List<Subscription>>(subscriptions));
        }

        /// <summary>
        /// Получение постов подписок
        /// </summary>
        [HttpGet]
        [Route("my/subscriptions/posts")]
        [ValidateModelState]
        [SwaggerOperation("GetSubscriptionsPosts")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<GetPostsResponse>), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<ActionResult> GetSubscriptionsPosts([FromQuery] int size = 10, [FromQuery] int page = 1, CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId ?? throw new InvalidOperationException("Current userId is null");

            var result = await Mediator.Send(new GetSubscriptionsPostsWithPaginationQuery { ProfileId = userId, PageSize = size, PageNumber = page }, cancellationToken);
            return Ok(_mapper.Map<GetPostsResponse>(result));
        }


        /// <summary>
        /// Получение постов текущего пользователя
        /// </summary>
        [HttpGet]
        [Route("my/posts")]
        [ValidateModelState]
        [SwaggerOperation("GetMyPosts")]
        [SwaggerResponse(statusCode: 200, type: typeof(GetPostsResponse), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GetMyPosts([FromQuery] int size = 10, [FromQuery] int page = 1, [FromQuery] bool isVisible = true, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(
                new GetMyPostsWithPaginationQuery { PageSize = size, PageNumber = page, IsVisible = isVisible }, cancellationToken);

            return Ok(_mapper.Map<GetPostsResponse>(result));
        }
    }
}