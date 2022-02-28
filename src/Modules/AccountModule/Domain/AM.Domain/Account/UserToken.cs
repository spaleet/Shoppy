﻿using _0_Framework.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AM.Domain.Account;

public class UserToken : EntityBase
{
    [BsonElement("userId")]
    public string UserId { get; set; }

    [BsonElement("accessTokenHash")]
    public string AccessTokenHash { get; set; }

    [BsonElement("accessTokenExpiresDateTime")]
    public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

    [BsonElement("refreshTokenIdHash")]
    public string RefreshTokenIdHash { get; set; }

    [BsonElement("refreshTokenIdHashSource")]
    public string RefreshTokenIdHashSource { get; set; }

    [BsonElement("refreshTokenExpiresDateTime")]
    public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }
}