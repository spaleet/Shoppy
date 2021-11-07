﻿using System.Threading;
using System.Threading.Tasks;
using _0_Framework.Application.ErrorMessages;
using _0_Framework.Application.Exceptions;
using _0_Framework.Application.Wrappers;
using _0_Framework.Domain.IGenericRepository;
using Ardalis.GuardClauses;
using MediatR;
using SM.Application.Contracts.ProductCategory.Commands;

namespace SM.Application.ProductCategory.CommandHandles
{
    public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Response<string>>
    {
        #region Ctor

        private readonly IGenericRepository<Domain.ProductCategory.ProductCategory> _productCategoryRepository;

        public DeleteProductCategoryCommandHandler(IGenericRepository<Domain.ProductCategory.ProductCategory> productCategoryRepository)
        {
            _productCategoryRepository = Guard.Against.Null(productCategoryRepository, nameof(_productCategoryRepository));
        }

        #endregion

        public async Task<Response<string>> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var productCategory = await _productCategoryRepository.GetEntityById(request.ProductCategoryId);

            if (productCategory is null)
                throw new NotFoundApiException();

            await _productCategoryRepository.FullDelete(productCategory.Id);
            await _productCategoryRepository.SaveChanges();

            return new Response<string>(ApplicationErrorMessage.RecordDeletedMessage);
        }
    }
}