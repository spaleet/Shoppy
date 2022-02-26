﻿namespace AM.Application.Contracts.Account.DTOs;

public class AccountDto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("fullName")]
    public string FullName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("avatarPath")]
    public string AvatarPath { get; set; }

    [JsonProperty("registerDate")]
    public string RegisterDate { get; set; }
}