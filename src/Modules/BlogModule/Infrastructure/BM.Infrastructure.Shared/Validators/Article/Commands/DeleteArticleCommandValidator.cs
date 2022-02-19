﻿using BM.Application.Contracts.Article.Commands;

namespace BM.Infrastructure.Shared.Validators.Article.Commands;

public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(p => p.ArticleId)
            .RequiredValidator("شناسه دسته بندی");
    }
}

