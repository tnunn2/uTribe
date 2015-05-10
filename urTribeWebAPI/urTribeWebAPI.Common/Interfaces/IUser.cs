using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Common
{
    public interface IUser : IDBRepositoryObject
    {
        string Token { get; }
        string Name { get; set; }
        UserCurrentStatus Status { get; set; }
        string InvitesChannel { get; set; }
        List<string> Channels { get; }
    }
}
