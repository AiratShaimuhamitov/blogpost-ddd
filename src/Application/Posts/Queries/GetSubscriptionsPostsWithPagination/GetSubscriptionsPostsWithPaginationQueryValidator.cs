using FluentValidation;
using Blogpost.Application.Posts.Queries.GetMyPostsWithPagination;

namespace Blogpost.Application.Posts.Queries.GetSubscriptionsPostsWithPagination;

public class GetSubscriptionsPostsWithPaginationQueryValidator : AbstractValidator<GetSubscriptionsPostsWithPaginationQuery>
{
    public GetSubscriptionsPostsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}