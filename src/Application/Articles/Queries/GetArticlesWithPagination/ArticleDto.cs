namespace Blogpost.Application.Articles.Queries.GetArticlesWithPagination
{
    public record ArticleDto
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}