﻿
using _0_Framework.Application.Utilities.ImageRelated;
using AutoMapper;
using SM.Application.Contracts.Slider.Commands;
using System.IO;

namespace SM.Application.Slider.CommandHandles;

public class CreateSliderCommandHandler : IRequestHandler<CreateSliderCommand, Response<string>>
{
    #region Ctor

    private readonly IGenericRepository<Domain.Slider.Slider> _sliderRepository;
    private readonly IMapper _mapper;

    public CreateSliderCommandHandler(IGenericRepository<Domain.Slider.Slider> sliderRepository, IMapper mapper)
    {
        _sliderRepository = Guard.Against.Null(sliderRepository, nameof(_sliderRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<Response<string>> Handle(CreateSliderCommand request, CancellationToken cancellationToken)
    {
        var slider =
            _mapper.Map(request.Slider, new Domain.Slider.Slider());

        var imagePath = Guid.NewGuid().ToString("N") + Path.GetExtension(request.Slider.ImageFile.FileName);

        request.Slider.ImageFile.AddImageToServer(imagePath, "wwwroot/slider/original/", 200, 200, "wwwroot/slider/thumbnail/");
        slider.ImagePath = imagePath;

        await _sliderRepository.InsertEntity(slider);
        await _sliderRepository.SaveChanges();

        return new Response<string>(ApplicationErrorMessage.OperationSucceddedMessage);
    }
}