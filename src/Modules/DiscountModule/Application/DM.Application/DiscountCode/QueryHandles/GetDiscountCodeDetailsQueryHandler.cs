﻿using DM.Application.Contracts.DiscountCode.DTOs;
using DM.Application.Contracts.DiscountCode.Queries;

namespace DM.Application.DiscountCode.QueryHandles;
public class GetDiscountCodeDetailsQueryHandler : IRequestHandler<GetDiscountCodeDetailsQuery, EditDiscountCodeDto>
{
    #region Ctor

    private readonly IRepository<Domain.DiscountCode.DiscountCode> _discountCodeRepository;
    private readonly IMapper _mapper;

    public GetDiscountCodeDetailsQueryHandler(IRepository<Domain.DiscountCode.DiscountCode> discountCodeRepository, IMapper mapper)
    {
        _discountCodeRepository = Guard.Against.Null(discountCodeRepository, nameof(_discountCodeRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<EditDiscountCodeDto> Handle(GetDiscountCodeDetailsQuery request, CancellationToken cancellationToken)
    {
        var discountCode = await _discountCodeRepository.FindByIdAsync(request.Id);

        if (discountCode is null)
            throw new NotFoundApiException();

        return _mapper.Map<EditDiscountCodeDto>(discountCode);
    }
}