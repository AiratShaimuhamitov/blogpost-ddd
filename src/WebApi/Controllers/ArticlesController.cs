using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blogpost.Application.Articles.Queries.GetArticlesWithPagination;
using Blogpost.WebApi.Attributes;
using Blogpost.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Blogpost.WebApi.Controllers
{
    [Authorize]
    public class ArticlesController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public ArticlesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Получение статей
        /// </summary>
        [HttpGet]
        [ValidateModelState]
        [SwaggerOperation("GetArticles")]
        [SwaggerResponse(statusCode: 200, type: typeof(GetArticlesResponse), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(Error), description: "Internal Server Error")]
        public async Task<IActionResult> GetArticles([FromQuery] int size = 10, [FromQuery] int page = 1, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(
                new GetArticlesWithPaginationQuery { PageSize = size, PageNumber = page }, cancellationToken);

            return Ok(_mapper.Map<GetArticlesResponse>(result));
        }
    }
}
