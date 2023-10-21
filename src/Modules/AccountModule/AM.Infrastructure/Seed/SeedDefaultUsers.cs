﻿using _0_Framework.Application.Extensions;
using AM.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AM.Infrastructure.Seed;

public static class SeedDefaultUsers
{
    public static async Task SeedAdminAsync(UserManager<Domain.Account.Account> userManager)
    {
        var adminUser = new Domain.Account.Account
        {
            Id = Guid.Parse(SeedUserIdConstants.AdminUser),
            UserName = Generator.UserName(),
            Email = "ali@gmail.com",
            FirstName = "ادمین",
            LastName = "ادمینی",
            PhoneNumber = "09123456789",
            EmailConfirmed = true
        };
        var user = await userManager.FindByEmailAsync(adminUser.Email);
        if (user == null)
        {
            await userManager.CreateAsync(adminUser, "123Pa$$word!");
            await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
            await userManager.AddToRoleAsync(adminUser, Roles.BasicUser.ToString());
        }
    }

    public static async Task SeedBasicUserAsync(UserManager<Domain.Account.Account> userManager)
    {
        var defaultUser = new Domain.Account.Account
        {
            Id = Guid.Parse(SeedUserIdConstants.BasicUser),
            UserName = Generator.UserName(),
            Email = "user1@g.com",
            FirstName = "کاربر",
            LastName = "کاربری",
            PhoneNumber = "09223456789",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if (user == null)
        {
            await userManager.CreateAsync(defaultUser, "123Pa$$word!");
            await userManager.AddToRoleAsync(defaultUser, Roles.BasicUser.ToString());
        }
    }
}

public class SeedUserIdConstants
{
    public const string AdminUser = "f949495a-d5a5-426b-8abc-1ad49426134d";

    public const string BasicUser = "0a0f7fa5-98ee-4080-af6c-c46da295e5e1";
}