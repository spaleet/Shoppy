﻿using _0_Framework.Application.Exceptions;
using AM.Application.Contracts.Account.DTOs;
using AM.Application.Contracts.Account.Queries;

namespace AM.Application.Account.QueryHandles;

public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQuery, Response<EditAccountDto>>
{
    #region Ctor

    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Account.Account> _userManager;

    public GetAccountDetailsQueryHandler(IMapper mapper,
                                         UserManager<Domain.Account.Account> userManager)
    {
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
        _userManager = Guard.Against.Null(userManager, nameof(_userManager));
    }

    #endregion Ctor

    public async Task<Response<EditAccountDto>> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _userManager.FindByIdAsync(request.UserId);

        if (account is null)
            throw new NotFoundApiException();

        var mappedAccount = _mapper.Map<EditAccountDto>(account);

        return new Response<EditAccountDto>(mappedAccount);
    }
}
