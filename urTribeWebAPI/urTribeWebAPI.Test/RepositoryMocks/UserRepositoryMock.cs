using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.DAL.Interfaces;


namespace urTribeWebAPI.Test.RepositoryMocks
{
    class UserRepositoryMock<userImpl> : IUserRepository where userImpl : IUser
    {
        #region Properties
        public static bool ThrowException
        {
            get;
            set;
        }
        public static IEnumerable<IUser> ListOfUsers
        {
            get;
            set;
        }
        public static IEnumerable<IEvent> ListOfEvents
        {
            get;
            set;
        }

        public static Guid UsrId
        {
            get;
            set;
        }
        public static Guid FriendId
        {
            get;
            set;
        }
        public static IUser User
        {
            get;
            set;
        }
        #endregion


        #region Public Methods
        public void Add(IUser poco)
        {
            if (ThrowException)
                throw new Exception();
            else User = poco;
        }
        public void Remove(IUser poco)
        {
            if (ThrowException)
                throw new Exception();
        }
        public IEnumerable<IUser> Find(System.Linq.Expressions.Expression<Func<IUser, bool>> predicate)
        {
            if (ThrowException)
                throw new Exception();

            return ListOfUsers;
        }
        public void AddToContactList(Guid usrId, Guid friendId)
        {
            if (ThrowException)
                throw new Exception();
        }
        public IEnumerable<IUser> RetrieveContacts(Guid userId)
        {
            if (ThrowException)
                throw new Exception();

            return ListOfUsers;
        }
        public void RemoveContact(Guid usrId, Guid friendId)
        {
            if (ThrowException)
                throw new Exception();
        }

        public IEnumerable<IEvent> RetrieveAllEventsByStatus(Guid usrId, EventAttendantsStatus status)
        {
            if (ThrowException)
                throw new Exception();

            return ListOfEvents;
        }

        #endregion
    }
}
