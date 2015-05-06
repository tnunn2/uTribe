﻿using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Common.Interfaces
{
    public interface IEvent : IDBRepositoryObject
    {
        string VenueName { get; set; }
        List<IUser> invitedUsers { get; set; }
        List<IUser> attendingUsers{get; set;}
    }
}