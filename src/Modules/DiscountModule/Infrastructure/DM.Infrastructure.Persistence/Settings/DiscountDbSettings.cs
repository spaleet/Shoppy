﻿using _0_Framework.Infrastructure;

namespace DM.Infrastructure.Persistence.Settings;

public class DiscountDbSettings : BaseMongoDbSettings
{
    public string CommentCollection { get; set; }
}