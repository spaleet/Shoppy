﻿using _0_Framework.Application.ErrorMessages;
using _0_Framework.Application.Exceptions;
using _0_Framework.Application.Models.Paging;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Account.DTOs;
using AM.Application.Contracts.Account.Queries;
using Microsoft.EntityFrameworkCore;

namespace AM.Application.Account.QueryHandles;

public class FilterAccountsQueryHandler : IRequestHandler<FilterAccountsQuery, Response<FilterAccountDto>>
{
    #region Ctor

    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Account.Account> _userManager;

    public FilterAccountsQueryHandler(IMapper mapper,
                                         UserManager<Domain.Account.Account> userManager)
    {
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
        _userManager = Guard.Against.Null(userManager, nameof(_userManager));
    }

    #endregion Ctor

    public async Task<Response<FilterAccountDto>> Handle(FilterAccountsQuery request, CancellationToken cancellationToken)
    {
        var query = _userManager.Users.AsQueryable();

        #region filter

        if (!string.IsNullOrEmpty(request.Filter.FullName))
            query = query.Where(s => EF.Functions.Like(s.FirstName, $"%{request.Filter.FullName}%")
            || EF.Functions.Like(s.LastName, $"%{request.Filter.FullName}%"));

        if (!string.IsNullOrEmpty(request.Filter.Email))
            query = query.Where(s => EF.Functions.Like(s.Email, $"%{request.Filter.Email}%"));

        switch (request.Filter.SortDateOrder)
        {
            case PagingDataSortCreationDateOrder.DES:
                query = query.OrderByDescending(x => x.CreatedOn);
                break;

            case PagingDataSortCreationDateOrder.ASC:
                query = query.OrderBy(x => x.CreatedOn);
                break;
        }

        switch (request.Filter.SortIdOrder)
        {
            case PagingDataSortIdOrder.NotSelected:
                break;

            case PagingDataSortIdOrder.DES:
                query = query.OrderByDescending(x => x.Id);
                break;

            case PagingDataSortIdOrder.ASC:
                query = query.OrderBy(x => x.Id);
                break;
        }

        #endregion filter

        #region paging

        var pager = request.Filter.BuildPager(query.Count());

        var allEntities = (await
            query
            .Paging(pager)
            .ToListAsyncSafe())
            .Select(user =>
                _mapper.Map(user, new AccountDto()))
            .ToList();

        #endregion paging

        var returnData = request.Filter.SetData(allEntities).SetPaging(pager);

        if (returnData.Accounts is null)
            throw new ApiException(ApplicationErrorMessage.FilteredRecordsNotFoundMessage);

        if (returnData.PageId > returnData.GetLastPage() && returnData.GetLastPage() != 0)
            throw new NotFoundApiException();

        return new Response<FilterAccountDto>(returnData);
    }
}
