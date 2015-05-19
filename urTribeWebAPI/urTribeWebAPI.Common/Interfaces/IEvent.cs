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
        string Street1 { get; set; }
        string Street2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }


    }
}
