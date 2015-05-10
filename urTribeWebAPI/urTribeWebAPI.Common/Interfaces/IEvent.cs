using System;
using System.Collections.Generic;

namespace urTribeWebAPI.Common
{
    public interface IEvent : IDBRepositoryObject
    {
        bool Active { get; set; } 

        string VenueName { get; set; }

    }
}
