﻿using _0_Framework.Application.Models.Paging;
using AM.Domain.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OM.Application.Contracts.Order.Enums;

namespace OM.Application.Order.QueryHandles;

public class FilterOrdersQueryHandler : IRequestHandler<FilterOrdersQuery, Response<FilterOrderDto>>
{
    #region Ctor

    private readonly IRepository<Domain.Order.Order> _orderRepository;
    private readonly UserManager<Account> _userManager;
    private readonly IMapper _mapper;

    public FilterOrdersQueryHandler(IRepository<Domain.Order.Order> orderRepository,
                                    IMapper mapper,
                                    UserManager<Account> userManager)
    {
        _orderRepository = Guard.Against.Null(orderRepository, nameof(_orderRepository));
        _userManager = Guard.Against.Null(userManager, nameof(_userManager));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<Response<FilterOrderDto>> Handle(FilterOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _orderRepository.AsQueryable(cancellationToken: cancellationToken);

        #region filter

        if (!string.IsNullOrEmpty(request.Filter.UserNames))
            query = query.Where(s => EF.Functions.Like(s.UserId, $"%{request.Filter.UserNames}%") ||
             EF.Functions.Like(s.UserId, $"%{request.Filter.UserNames}%"));
        // TODO
        switch (request.Filter.SortDateOrder)
        {
            case PagingDataSortCreationDateOrder.DES:
                query = query.OrderByDescending(x => x.CreationDate);
                break;

            case PagingDataSortCreationDateOrder.ASC:
                query = query.OrderBy(x => x.CreationDate);
                break;
        }

        switch (request.Filter.PaymentState)
        {
            case FilterOrderPaymentStatus.All:
                break;

            case FilterOrderPaymentStatus.IsPaid:
                query = query.Where(x => x.IsPaid);
                break;

            case FilterOrderPaymentStatus.IsCanceled:
                query = query.Where(x => x.IsCanceled);
                break;

            case FilterOrderPaymentStatus.PaymentPending:
                query = query.Where(x => (!x.IsCanceled && !x.IsPaid));
                break;
        }

        #endregion filter

        #region paging

        var pager = request.Filter.BuildPager((await query.CountAsync()), cancellationToken);

        var allEntities =
             _orderRepository
             .ApplyPagination(query, pager, cancellationToken)
             .Select(x =>
                _mapper.Map(x, new OrderDto()))
             .ToList();

        for (int i = 0; i < allEntities.Count; i++)
        {
            var user = await _userManager.FindByIdAsync(allEntities[i].AccountId);

            allEntities[i].UserFullName = user?.FirstName + ' ' + user?.LastName;
        }

        #endregion paging

        var returnData = request.Filter.SetData(allEntities).SetPaging(pager);

        if (!returnData.Orders.Any())
            throw new NoContentApiException();

        if (returnData.PageId > returnData.GetLastPage() && returnData.GetLastPage() != 0)
            throw new NoContentApiException();

        return new Response<FilterOrderDto>(returnData);
    }
}