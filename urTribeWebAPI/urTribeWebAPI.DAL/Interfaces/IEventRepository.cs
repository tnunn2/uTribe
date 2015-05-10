using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;


namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IEventRepository : IRepository<IEvent>
    {
        void Add(IUser usr, IEvent evt);
        void Update(IEvent evt);
        Guid Owner(IEvent evt);
        bool Guest(IEvent evt, Guid userId);
        void LinkToEvent(IUser usr, IEvent evt);
        IEnumerable<IUser> AttendingByStatus(Guid eventId, EventAttendantsStatus attendStatus);
        void ChangeUserAttendStatus(Guid userId, Guid eventId, EventAttendantsStatus attendStatus);
    }
}
