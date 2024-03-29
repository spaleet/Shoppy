﻿using _0_Framework.Application.Exceptions;
using AM.Application.Account.DTOs;

namespace AM.Application.Account.Commands;

public record ChangePasswordCommand(ChangePasswordDto Password, string UserId) : IRequest<ApiResult>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApiResult>
{
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Account.Account> _userManager;

    public ChangePasswordCommandHandler(IMapper mapper,
                                         UserManager<Domain.Account.Account> userManager)
    {
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
        _userManager = Guard.Against.Null(userManager, nameof(_userManager));
    }

    public async Task<ApiResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            throw new ApiException("کاربری با این مشخصات پیدا نشد");

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password.OldPassword);

        if (!isCorrectPassword)
            throw new ApiException("رمز فعلی وارد شده اشتباه است");

        var verifyPasswordResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password.NewPassword);

        if (verifyPasswordResult == PasswordVerificationResult.Failed)
            throw new ApiException("رمز عبور جدید نباید با رمز عبور فعلی یکسان باشد");

        var changePasswordResult = await _userManager
            .ChangePasswordAsync(user, request.Password.OldPassword, request.Password.NewPassword);

        if (!changePasswordResult.Succeeded)
            throw new ApiException("عملیات با خطا مواجه شد");

        user.SerialNumber = Guid.NewGuid().ToString("N");

        await _userManager.UpdateAsync(user);

        return ApiResponse.Success("حساب کاربری شما با موفقیت ویرایش شد");
    }
}