﻿using _0_Framework.Application.Extensions;
using BM.Application.Contracts.ArticleCategory.Commands;

namespace BM.Application.ArticleCategory.CommandHandles;

public class EditArticleCategoryCommandHandler : IRequestHandler<EditArticleCategoryCommand, Response<string>>
{
    #region Ctor

    private readonly IGenericRepository<Domain.ArticleCategory.ArticleCategory> _articleCategoryRepository;
    private readonly IMapper _mapper;

    public EditArticleCategoryCommandHandler(IGenericRepository<Domain.ArticleCategory.ArticleCategory> articleCategoryRepository, IMapper mapper)
    {
        _articleCategoryRepository = Guard.Against.Null(articleCategoryRepository, nameof(_articleCategoryRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<Response<string>> Handle(EditArticleCategoryCommand request, CancellationToken cancellationToken)
    {
        var ArticleCategory = await _articleCategoryRepository.GetEntityById(request.ArticleCategory.Id);

        if (ArticleCategory is null)
            throw new NotFoundApiException();

        if (_articleCategoryRepository.Exists(x => x.Title == request.ArticleCategory.Title && x.Id != request.ArticleCategory.Id))
            throw new ApiException(ApplicationErrorMessage.IsDuplicatedMessage);

        _mapper.Map(request.ArticleCategory, ArticleCategory);

        if (request.ArticleCategory.ImageFile != null)
        {
            var imagePath = DateTime.Now.ToFileName() + Path.GetExtension(request.ArticleCategory.ImageFile.FileName);

            request.ArticleCategory.ImageFile.AddImageToServer(imagePath, PathExtension.ArticleCategoryImage,
                200, 200, PathExtension.ArticleCategoryThumbnailImage, ArticleCategory.ImagePath);

            ArticleCategory.ImagePath = imagePath;
        }

        _articleCategoryRepository.Update(ArticleCategory);
        await _articleCategoryRepository.SaveChanges();

        return new Response<string>();
    }
}