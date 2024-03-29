﻿using AM.Application.Models;

namespace AM.Application.Account.DTOs;

public class AuthenticateUserResponseDto
{
    public AuthenticateUserResponseDto(JwtTokenResponse token)
    {
        AccessToken = token.AccessToken;
        RefreshToken = token.RefreshToken;
    }

    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }

    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; }
}