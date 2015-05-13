using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Common
{
    public interface IEvent : IDBRepositoryObject
    {
        bool Active { get; set; }
        string Name { get; set; }
        long Time { get; set; }
        string Location { get; set; }
        Address LocationAddress { get; set; }

    }
}
