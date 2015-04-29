using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;


namespace urTribeWebAPI.DAL.Interfaces
{
    public interface IUserRepository : IRepository<IUser>
    {
        void Add(IUser usr);
        void AddToContactList(Guid usrId, Guid friendId);
        IEnumerable<IUser> RetrieveContacts(Guid userId);
        void RemoveContact(Guid usrId, Guid friendId);
    }
}
