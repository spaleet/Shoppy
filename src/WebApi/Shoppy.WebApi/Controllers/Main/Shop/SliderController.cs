﻿using _01_Shoppy.Query.Contracts.Slider;
using Shoppy.WebApi.Endpoints.Main.Shop;

namespace Shoppy.WebApi.Controllers.Main.Shop;
public class SliderController : BaseApiController
{
    #region Ctor 

    private readonly ISliderQuery _sliderQuery;

    public SliderController(ISliderQuery sliderQuery)
    {
        _sliderQuery = Guard.Against.Null(sliderQuery, nameof(_sliderQuery)); ;
    }

    #endregion

    #region Get Sliders

    /// <summary>
    ///    دریافت اسلایدر ها
    /// </summary>
    /// <response code="200">Success</response>
    [HttpGet(MainShopApiEndpoints.Slider.GetSliders)]
    public async Task<IActionResult> GetSliders()
    {
        var res = await _sliderQuery.GetSliders();

        return JsonApiResult.Success(res);
    }

    #endregion
}