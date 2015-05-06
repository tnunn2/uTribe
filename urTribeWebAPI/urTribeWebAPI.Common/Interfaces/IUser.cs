﻿using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Common.Interfaces
{
    public interface IUser : IDBRepositoryObject
    {
        List<string> Channels { get; }
        string Token { get; }
        string Name { get; set; }
        string InvitesChannel { get; set; }
    }
}