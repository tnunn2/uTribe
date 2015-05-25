using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Common
{
    public interface IUser : IDBRepositoryObject
    {
        string Token { get; }
        string Name { get; set; }
        UserCurrentStatus Status { get; set; }
        string UserChannel { get; set; }
        List<string> AuthenticatedChannels { get; set;  }
    }
}
