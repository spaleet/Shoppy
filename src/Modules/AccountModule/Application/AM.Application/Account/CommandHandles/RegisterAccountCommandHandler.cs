﻿using _0_Framework.Application.Exceptions;
using AM.Application.Contracts.Account.DTOs;
using AM.Domain.Enums;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace AM.Application.Account.CommandHandles;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, ApiResult<RegisterAccountResponseDto>>
{
    #region Ctor

    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Account.Account> _userManager;
    private readonly SignInManager<Domain.Account.Account> _signInManager;

    public RegisterAccountCommandHandler(IMapper mapper,
                                         UserManager<Domain.Account.Account> userManager,
                                         SignInManager<Domain.Account.Account> signInManager)
    {
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
        _userManager = Guard.Against.Null(userManager, nameof(_userManager));
        _signInManager = Guard.Against.Null(signInManager, nameof(_signInManager));
    }

    #endregion Ctor

    public async Task<ApiResult<RegisterAccountResponseDto>> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Account.Email);

        if (userWithSameEmail != null)
            throw new ApiException($"ایمیل وارد شده {request.Account.Email} تکراری می‌ باشد");

        var user = _mapper.Map(request.Account, new Domain.Account.Account());

        user.SerialNumber = Guid.NewGuid().ToString("N");

        var result = await _userManager.CreateAsync(user, request.Account.Password);

        if (!result.Succeeded)
            throw new ApiException(result.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, Roles.BasicUser.ToString());
        await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());

        var userToConfirm = await _userManager.FindByEmailAsync(user.Email);

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(userToConfirm);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id.ToString()));

        return ApiResponse.Success<RegisterAccountResponseDto>(new RegisterAccountResponseDto(token,
                                                                                              userId,
                                                                                              user.Email,
                                                                                              $"{user.FirstName} {user.LastName}"));
    }
}
