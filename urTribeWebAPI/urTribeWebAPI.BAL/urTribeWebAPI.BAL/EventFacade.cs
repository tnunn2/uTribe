using System;
using System.Collections.Generic;
using urTribeWebAPI.Common;
using urTribeWebAPI.Common.Interfaces;
using urTribeWebAPI.Common.Logging;
using urTribeWebAPI.DAL.Interfaces;
using urTribeWebAPI.DAL.Factory;
using urTribeWebAPI.Common.Concrete;

namespace urTribeWebAPI.BAL
{
    public class EventFacade : IDisposable
    {
        #region Member Variables
        private readonly IEventRepository _repository; 
        #endregion

        #region Properties
        private IEventRepository EvtRepository
        {
            get
            {
                return _repository;
            }
        }
        #endregion

        #region Constructors
        public EventFacade ()
        {
                var factory = RepositoryFactory.Instance;
                _repository = factory.Create<IEventRepository>();
        }
        #endregion

        #region Public Methods
        public Guid UpdateEvent(Guid userId, IEvent evt)
        {
            try
            {
                if (userId == EvtRepository.Owner(evt))
                {
                    EvtRepository.Update(evt);
                    return evt.ID;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "UpdateEvent", Exception = ex };
            }
            return new Guid("99999999-9999-9999-9999-999999999999");
        }
        public IEvent FindEvent(Guid eventId)
        {
            try
            {
                var eventList = EvtRepository.Find(evt => evt.ID == eventId);

                foreach (IEvent evt in eventList)
                    return evt;
                return null;
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "FindEvent", Exception = ex };
                return null;
            }
        }
        public void AddContactsToEvent(Guid userId, Guid eventId, List<Guid> contactList)
        {
            try
            {
                IEvent evt = FindEvent(eventId);

                if (userId != EvtRepository.Owner(evt))
                  return;

                foreach (var contact in contactList)
                {
                    if (!EvtRepository.Guest(evt, contact))
                        continue;

                    using (UserFacade userFacade = new UserFacade())
                    {
                        IUser user = userFacade.FindUser(userId);
                        EvtRepository.LinkToEvent(user, evt);
                    }  
                }  
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "AddContactsToEvent", Exception = ex };
            }
        }
        public void ChangeContactAttendanceStatus (Guid userId, Guid eventId, EventAttendantsStatus attendStatus)
        {
            try
            {
                if (attendStatus == EventAttendantsStatus.All || attendStatus == EventAttendantsStatus.Cancel)
                    return;

                IEvent evt = FindEvent(eventId);
                if (!EvtRepository.Guest(evt, userId))
                    return;

                EvtRepository.ChangeUserAttendStatus(userId, eventId, attendStatus);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "EventFacade", FaultMethod = "ChangeContactAttendanceStatus", Exception = ex };
            }
        }
        public void Dispose ()
        {
        }
        #endregion
    }
}
