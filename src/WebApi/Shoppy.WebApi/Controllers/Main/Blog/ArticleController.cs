﻿using _01_Shoppy.Query.Models.Blog.Article;
using _01_Shoppy.Query.Queries.Article;
using _01_Shoppy.Query.Queries.Blog.Article;

namespace Shoppy.WebApi.Controllers.Main.Article;

[SwaggerTag("مقاله ها")]
public class ArticleController : BaseApiController
{
    #region Search

    [HttpGet(MainBlogEndpoints.Article.Search)]
    [SwaggerOperation(Summary = "جستجو", Tags = new[] { "Article" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(400, "error : no data with requested filter")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(SearchArticleQueryModel), 200)]
    [ProducesResponseType(typeof(ApiResult), 400)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> Search([FromQuery] SearchArticleQueryModel search, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new SearchArticleQuery(search), cancellationToken);

        return SuccessResult(res);
    }

    #endregion

    #region Get Article Details

    [HttpGet(MainBlogEndpoints.Article.GetArticleDetails)]
    [SwaggerOperation(Summary = "دریافت جزییات مقاله", Tags = new[] { "Article" })]
    [SwaggerResponse(200, "success")]
    [SwaggerResponse(404, "not-found")]
    [ProducesResponseType(typeof(ArticleDetailsQueryModel), 200)]
    [ProducesResponseType(typeof(ApiResult), 404)]
    public async Task<IActionResult> GetArticleDetails([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new GetArticleDetailsQuery(slug), cancellationToken);

        return SuccessResult(res);
    }

    #endregion

    #region Get Latest Articles

    [HttpGet(MainBlogEndpoints.Article.GetLatestArticles)]
    [SwaggerOperation(Summary = "دریافت جدید ترین مقالات", Tags = new[] { "Article" })]
    [SwaggerResponse(200, "success")]
    [ProducesResponseType(typeof(IEnumerable<ArticleQueryModel>), 200)]
    public async Task<IActionResult> GetLatestArticles(CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new GetLatestArticlesQuery(), cancellationToken);

        return SuccessResult(res);
    }

    #endregion

    #region Get Related Articles

    [HttpGet(MainBlogEndpoints.Article.GetRelatedArticles)]
    [SwaggerOperation(Summary = "دریافت مقالات مرتبط", Tags = new[] { "Article" })]
    [SwaggerResponse(200, "success")]
    [ProducesResponseType(typeof(IEnumerable<ArticleQueryModel>), 200)]
    public async Task<IActionResult> GetRelatedArticles([FromRoute] string categoryId, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new GetRelatedArticlesQuery(categoryId), cancellationToken);

        return SuccessResult(res);
    }

    #endregion
}
