using System;
using System.Collections.Generic;
using urTribeWebAPI.Common.Interfaces;
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

        public static Guid UsrId
        {
            get;
            set;
        }

        public static IUser User
        {
            get;
            set;
        }

        public static IEvent evt
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
        }

        public void LinkToEvent(IUser usr, IEvent evt)
        {
            if (ThrowException)
                throw new Exception();
        }

        public void ChangeUserAttendStatus (IUser usr, IEvent evt)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEvent> Find(System.Linq.Expressions.Expression<Func<IEvent, bool>> predicate)
        {
            if (ThrowException)
                throw new Exception();

            return ListOfEvents;
        }

        public void Remove(IEvent poco)
        {
            if (ThrowException)
                throw new Exception();
        }
        #endregion
    }
}
