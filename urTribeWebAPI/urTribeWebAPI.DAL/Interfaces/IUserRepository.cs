using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;


namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IUserRepository : IRepository<IUser>
    {
        void Add(IUser usr);
        void AddToContactList(Guid usrId, Guid friendId);
        void AddFriendToGroup(Guid usrId, Guid contactID, Guid groupId);
        IEnumerable<IUser> RetrieveContacts(Guid userId);
        void RemoveContactFromGroup(Guid ID, Guid contactID, Guid groupId);
        void RemoveContact(Guid usrId, Guid friendId);
    }
}
