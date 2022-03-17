﻿using _0_Framework.Infrastructure;
using DM.Domain.ProductDiscount;
using IM.Application.Contracts.Inventory.Helpers;
using IM.Domain.Inventory;
using MongoDB.Driver;
using SM.Domain.Product;

namespace OM.Application.Order.QueryHandles;

public class CheckoutCartQueryHandler : IRequestHandler<CheckoutCartQuery, Response<CartDto>>
{
    #region Ctor

    private readonly IRepository<Inventory> _inventoryRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ProductDiscount> _productDiscountRepository;
    private readonly IInventoryHelper _inventoryHelper;
    private readonly IMapper _mapper;

    public CheckoutCartQueryHandler(IRepository<Product> productRepository,
                                                    IRepository<Inventory> inventoryRepository,
                                                    IRepository<ProductDiscount> productDiscountRepository,
                                                    IInventoryHelper inventoryHelper,
                                                    IMapper mapper)
    {
        _inventoryRepository = Guard.Against.Null(inventoryRepository, nameof(_inventoryRepository));
        _productRepository = Guard.Against.Null(productRepository, nameof(_productRepository));
        _inventoryHelper = Guard.Against.Null(inventoryHelper, nameof(_inventoryHelper));
        _productDiscountRepository = Guard.Against.Null(productDiscountRepository, nameof(_productDiscountRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<Response<CartDto>> Handle(CheckoutCartQuery request, CancellationToken cancellationToken)
    {
        var cart = new CartDto();

        var productDiscounts = await _productDiscountRepository
                            .AsQueryable()
                            .Where(x => !x.IsExpired)
                            .Select(x => new { x.Rate, x.ProductId })
                            .ToListAsyncSafe();

        foreach (var cartItem in request.Items)
        {
            if (cartItem.Count <= 0)
                continue;

            var itemToReturn = new CartItemDto();

            if (!(await _inventoryRepository.ExistsAsync(x => x.ProductId == cartItem.ProductId)))
                continue;

            #region check inventory

            var filter = Builders<Inventory>.Filter.Eq(x => x.ProductId, cartItem.ProductId);

            var itemInventory = await _inventoryRepository.GetByFilter(filter);

            if (!(await _inventoryHelper.IsInStock(itemInventory?.Id)))
                continue;

            long inventoryCount = await _inventoryHelper.CalculateCurrentCount(itemInventory?.Id);

            if (inventoryCount <= 0)
                continue;

            if ((inventoryCount < cartItem.Count))
                continue;

            var product = await _productRepository.GetByIdAsync(itemInventory?.ProductId);

            itemToReturn.UnitPrice = itemInventory.UnitPrice;

            _mapper.Map(product, itemToReturn);
            _mapper.Map(cartItem, itemToReturn);

            itemToReturn.CalculateTotalItemPrice();

            #endregion

            #region discount

            var productDiscount = productDiscounts.FirstOrDefault(x => x.ProductId == cartItem.ProductId);

            if (productDiscount is not null)
            {
                itemToReturn.DiscountRate = productDiscount.Rate;
                itemToReturn.UnitPriceWithDiscount = itemToReturn.UnitPrice - ((itemToReturn.UnitPrice * itemToReturn.DiscountRate) / 100);
            }
            else
            {
                itemToReturn.UnitPriceWithDiscount = itemToReturn.UnitPrice;
            }

            itemToReturn.DiscountAmount = ((itemToReturn.TotalItemPrice * itemToReturn.DiscountRate) / 100);
            itemToReturn.ItemPayAmount = itemToReturn.TotalItemPrice - itemToReturn.DiscountAmount;

            #endregion

            cart.Items.Add(itemToReturn);
        }

        for (int i = 0; i < cart.Items.Count; i++)
        {
            cart.TotalAmount += cart.Items[i].TotalItemPrice;
            cart.PayAmount += cart.Items[i].ItemPayAmount;
            cart.DiscountAmount += cart.Items[i].DiscountAmount;
        }

        return new Response<CartDto>(cart);
    }
}