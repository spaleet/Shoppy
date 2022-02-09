﻿using CM.Application.Contracts.Comment.Commands;

namespace CM.Infrastructure.Shared.Validators;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(p => p.Comment.Name)
            .RequiredValidator("نام");

        RuleFor(p => p.Comment.Email)
            .CustomEmailAddressValidator();

        RuleFor(p => p.Comment.Text)
            .RequiredValidator("نام")
            .MaxLengthValidator("متن نظر", 500);

        RuleFor(p => p.Comment.OwnerRecordId)
            .RangeValidator("شناسه محصول/مقاله", 1, 100000);

        RuleFor(p => p.Comment.ParentId)
            .RangeValidator("شناسه والد", 0, 100000);
    }
}