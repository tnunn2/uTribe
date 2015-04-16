
using System;
using System.Collections.Generic;
namespace urTribeWebAPI.Common.Interfaces
{
    public interface IEvent : IDBRepositoryObject
    {

        public List<IUser> invitedUsers { get; set; }
        public List<IUser> attendingUsers{get; set;}
        public Guid ID {get; set;}
    }
}
