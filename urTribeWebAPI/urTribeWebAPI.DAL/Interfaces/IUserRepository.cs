using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;


namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IUserRepository : IRepository<IUser>
    {
        void Add(IUser usr);
        void Update(IUser usr);
        void AddToContactList(Guid usrId, Guid friendId);
        IEnumerable<IUser> RetrieveContacts(Guid userId);
        void RemoveContact(Guid usrId, Guid friendId);
        IEnumerable<IEvent> RetrieveAllEventsByStatus(Guid usrId, EventAttendantsStatus status);
        EventAttendantsStatus RetrieveEventStatus (Guid usrId, Guid eventId);
    }
}
