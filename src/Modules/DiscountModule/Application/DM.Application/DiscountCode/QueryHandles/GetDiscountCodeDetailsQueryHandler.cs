﻿using DM.Application.Contracts.DiscountCode.DTOs;
using DM.Application.Contracts.DiscountCode.Queries;

namespace DM.Application.DiscountCode.QueryHandles;
public class GetDiscountCodeDetailsQueryHandler : IRequestHandler<GetDiscountCodeDetailsQuery, Response<EditDiscountCodeDto>>
{
    #region Ctor

    private readonly IGenericRepository<Domain.DiscountCode.DiscountCode> _discountCodeRepository;
    private readonly IMapper _mapper;

    public GetDiscountCodeDetailsQueryHandler(IGenericRepository<Domain.DiscountCode.DiscountCode> discountCodeRepository, IMapper mapper)
    {
        _discountCodeRepository = Guard.Against.Null(discountCodeRepository, nameof(_discountCodeRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<Response<EditDiscountCodeDto>> Handle(GetDiscountCodeDetailsQuery request, CancellationToken cancellationToken)
    {
        var discountCode = await _discountCodeRepository.GetByIdAsync(request.Id);

        if (discountCode is null)
            throw new NotFoundApiException();

        var mappedDiscountCode = _mapper.Map<EditDiscountCodeDto>(discountCode);

        return new Response<EditDiscountCodeDto>(mappedDiscountCode);
    }
}