using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;
using urTribeWebAPI.DAL.Interfaces;

namespace urTribeWebAPI.Test.RepositoryMocks
{
    public class EventRepositoryMock<eventImpl> : IEventRepository where eventImpl : IEvent
    {
        #region Properties
        public static bool ThrowException
        {
            get;
            set;
        }

        public static IEnumerable<IEvent> ListOfEvents
        {
            get;
            set;
        }

        public static IEnumerable<IUser> ListOfUsers
        {
            get;
            set;
        }

        public static Guid UserId
        {
            get;
            set;
        }

        public static IUser User
        {
            get;
            set;
        }

        public static IEvent Evt
        {
            get;
            set;
        }

        public static Guid EventId
        {
            get;
            set;
        }

        public static Guid OwnerId
        {
            get;
            set;
        }

        public static bool IsGuest 
        { 
            get; 
            set; 
        }
        #endregion

        #region Public Methods
        public void Add(IUser usr, IEvent evt)
        {
            if (ThrowException)
                throw new Exception();
            Evt = evt;
        }
        public void Update(IEvent evt)
        {
            if (ThrowException)
                throw new Exception();
            Evt = evt;
        }
        public void LinkToEvent(IUser usr, IEvent evt)
        {
            if (ThrowException)
                throw new Exception();
        }
        public void ChangeUserAttendStatus(Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            if (ThrowException)
                throw new Exception();
        }
        public IEnumerable<IEvent> Find(System.Linq.Expressions.Expression<Func<IEvent, bool>> predicate)
        {
            if (ThrowException)
                throw new Exception();

            return ListOfEvents;
        }
        public IEnumerable<IUser> AttendingByStatus(Guid eventId, EventAttendantsStatus attendStatus)
        {
            if (ThrowException)
                throw new Exception();

            return ListOfUsers;

        }
        public void Remove(IEvent poco)
        {
            if (ThrowException)
                throw new Exception();
        }
        public Guid Owner(IEvent evt)
        {
            if (ThrowException)
                throw new Exception();

            return OwnerId;
        }
        public bool Guest(IEvent evt, Guid userId)
        {
            if (ThrowException)
                throw new Exception();

            return IsGuest;
        }
        #endregion

    }
}
